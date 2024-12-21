using Microsoft.Extensions.Logging;
using SMTP.Impostor.Messages;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SMTP.Impostor.Sockets
{

    internal class SocketHandler :
        ISMTPImpostorSocketHandler
    {
        public const int SessionReadBufferSize = 4 * 1024;

        internal const string QUOTE_UTF8 = "=?utf-8?q?";
        internal const string BASE64_UTF8 = "=?utf-8?b?";
        internal const string ENCODED_WORD_PATTERN = @"=\?([a-z0-9\-]+)\?([qb])\?(.+?)\?=";

        public const string COMMAND_EHLO = "EHLO";
        public const string COMMAND_HELO = "HELO";
        public const string COMMAND_MAIL = "MAIL";
        public const string COMMAND_RCPT = "RCPT";
        public const string COMMAND_DATA = "DATA";
        public const string COMMAND_QUIT = "QUIT";
        public const string COMMAND_RSET = "RSET";
        public const string COMMAND_NOOP = "NOOP";

        readonly ISMTPImpostorSocket _socket;
        readonly ILogger _logger;
        readonly NetworkStream _networkStream;

        Action<SMTPImpostorMessage> _onMessage;

        public SocketHandlerStates Status { get; set; }
        public IPAddress ClientAddress { get; set; }
        public string ClientId { get; set; }

        public SocketHandler(
            ISMTPImpostorSocket socket,
            ILogger logger
            )
        {
            _socket = socket;
            _logger = logger;
            _networkStream = _socket.GetNetworkStream();

            ClientAddress = socket.RemoteEndPoint.Address;
        }

        public async Task HandleAsync(Action<SMTPImpostorMessage> onMessage)
        {
            _onMessage = onMessage;

            await WriteAsync(
                            ReplyCodes.Ready_220,
                            string.Format(Resources.Ready_220, ClientAddress));

            Status = SocketHandlerStates.Connected;
            //_raiseHostEvent(SocketHandlerStates.SessionConnected, this);

            await ReadAsync();
        }

        /// <summary>
        ///   <para>Read the data</para>
        /// </summary>
        /// <param name="result"></param>
        //[DebuggerStepThrough]
        async Task ReadAsync()
        {
            var terminator = SMTPImpostorMessage.LINE_TERMINATOR;
            var readBuffer = new byte[SessionReadBufferSize];
            var dataBuffer = new StringBuilder();

            MailAddress from = null;
            List<MailAddress> recipients = null;

            while (_networkStream.CanWrite == true)
            {
                var read = await _networkStream
                    .ReadAsync(readBuffer, 0, readBuffer.Length);

                if (read == 0) continue;

                var readData = Encoding.UTF8
                    .GetString(readBuffer, 0, read);

                dataBuffer.Append(readData);

                // check for terminator
                if (!dataBuffer.EndsWith(terminator)) continue;

                // process data received not including the terminator
                var data = dataBuffer.ToString(0, dataBuffer.Length - terminator.Length);
                dataBuffer.Clear();

                if (Status == SocketHandlerStates.Data)
                {
                    var message = SMTPImpostorMessage.Parse(data);
                    // _logger.LogInformation("Session.Process Data: => {0}", data);

                    await WriteAsync(ReplyCodes.Completed_250);
                    Status = SocketHandlerStates.Identified;

                    _onMessage(message);

                    terminator = SMTPImpostorMessage.LINE_TERMINATOR;
                    continue;
                }

                // command expected
                var command = data.Length < 4
                            ? string.Empty
                            : data[..4].ToUpper();

                _logger.LogInformation("Session.Process Command: => {Command}", command);

                switch (command)
                {
                    case COMMAND_QUIT:
                        _networkStream.Close();
                        return;

                    case COMMAND_EHLO:
                    case COMMAND_HELO:
                        ClientId = data.Substring(4).Trim();
                        // _logger.LogInformation("Session.Process ClientId: => {0}", ClientId);

                        await WriteAsync(ReplyCodes.Completed_250, string.Format(Resources.Hello_250, ClientId));

                        Status = SocketHandlerStates.Identified;
                        //Host.RaiseEvent(HostEventTypes.SessionIdentified, this);
                        continue;

                    case COMMAND_NOOP:
                        await WriteAsync(ReplyCodes.Completed_250, string.Format(Resources.Hello_250, ClientId));
                        continue;

                    case COMMAND_RSET:
                        await WriteAsync(ReplyCodes.Completed_250, string.Format(Resources.Hello_250, ClientId));

                        Status = SocketHandlerStates.Identified;
                        continue;
                }

                if (Status < SocketHandlerStates.Identified)
                {
                    // commands beyond her need minimum of Identified state
                    await WriteAsync(ReplyCodes.CommandSequenceError_503, Resources.ExpectedHELO_503);
                    _networkStream.Close();
                    return;
                }

                switch (command)
                {
                    case COMMAND_MAIL:
                        // store the from address, in case its not in the header
                        from = data.Tail(":").ToMailAddress();
                        recipients = null;

                        // _logger.LogInformation("Session.Process From: => {0}", _from);

                        Status = SocketHandlerStates.Mail;
                        await WriteAsync(ReplyCodes.Completed_250);

                        continue;

                    case COMMAND_RCPT:

                        if (Status != SocketHandlerStates.Mail
                            && Status != SocketHandlerStates.Recipient)
                        {
                            await WriteAsync(ReplyCodes.CommandSequenceError_503);
                        }
                        else
                        {
                            if (recipients == null) recipients = new List<MailAddress>();
                            var recipient = data.Tail(":").ToMailAddress();
                            recipients.Add(recipient);

                            // _logger.LogInformation("Session.Process To: => {0}", to);

                            Status = SocketHandlerStates.Recipient;
                            await WriteAsync(ReplyCodes.Completed_250);
                        }

                        continue;
                    case COMMAND_DATA:

                        // request data
                        Status = SocketHandlerStates.Data;
                        await WriteAsync(ReplyCodes.StartInput_354);

                        terminator = SMTPImpostorMessage.DATA_TERMINATOR;

                        continue;
                    default:

                        await WriteAsync(ReplyCodes.CommandNotImplemented_502);

                        _networkStream.Close();
                        return;
                }
            }
        }

        async Task WriteAsync(string data)
        {
            // _logger.LogInformation("Session.Write: => {0}", data);

            data += SMTPImpostorMessage.LINE_TERMINATOR;

            var bytes = Encoding.UTF8.GetBytes(data);
            await _networkStream.WriteAsync(bytes, 0, bytes.Length);
        }

        async Task WriteAsync(ReplyCodes code, string description)
        {
            await WriteAsync(string.Format("{0:D} {1}", code, description));
        }

        async Task WriteAsync(ReplyCodes code)
        {
            await WriteAsync(code, Resources.Text[code]);
        }
    }
}
