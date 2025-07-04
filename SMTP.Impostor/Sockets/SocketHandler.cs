using Microsoft.Extensions.Logging;
using SMTP.Impostor.Messages;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

        Action<SMTPImpostorMessage> _onMessage;

        public SocketHandlerStates Status { get; private set; }
        public IPAddress ClientAddress { get; private set; }
        public string ClientId { get; private set; }

        public SocketHandler(
            ISMTPImpostorSocket socket,
            ILogger logger
            )
        {
            _socket = socket;
            _logger = logger;

            ClientAddress = socket.RemoteEndPoint.Address;
        }

        public async Task HandleAsync(Action<SMTPImpostorMessage> onMessage)
        {
            _onMessage = onMessage;

            using var networkStream = _socket.GetNetworkStream();

            await WriteAsync(
                networkStream,
                ReplyCodes.Ready_220,
                string.Format(Resources.Ready_220, ClientAddress)
            );

            Status = SocketHandlerStates.Connected;

            await ReadAsync(networkStream);

            Status = SocketHandlerStates.Disconnected;

            _logger.LogDebug("Message Handled");
        }

        /// <summary>
        ///   <para>Read the data</para>
        /// </summary>
        /// <param name="result"></param>
        //[DebuggerStepThrough]
        async Task ReadAsync(NetworkStream networkStream)
        {
            var terminator = SMTPImpostorMessage.LINE_TERMINATOR;
            var readBuffer = ArrayPool<byte>.Shared.Rent(SessionReadBufferSize); 
            var dataBuffer = new StringBuilder();

            MailAddress from = null;
            List<MailAddress> recipients = null;


            while (networkStream.CanRead)
            {
                using var cancelSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));

                var memory = readBuffer.AsMemory();
                var read = await networkStream.ReadAsync(memory, cancelSource.Token);

                cancelSource.Dispose();

                if (read == 0)
                {
                    await Task.Delay(1);
                    continue;
                }

                var readData = Encoding.UTF8
                    .GetString(memory.Span.Slice(0, read));

                dataBuffer.Append(readData);

                // check for terminator
                if (!dataBuffer.EndsWith(terminator)) continue;

                // process data received not including the terminator
                var data = dataBuffer.ToString(0, dataBuffer.Length - terminator.Length);
                dataBuffer.Clear();

                if (Status == SocketHandlerStates.Data)
                {
                    var message = SMTPImpostorMessage.Parse(data);
                    _logger.LogDebug("Session.Process Data: => {Data}", data);

                    await WriteAsync(networkStream, ReplyCodes.Completed_250);
                    Status = SocketHandlerStates.Identified;
                    terminator = SMTPImpostorMessage.LINE_TERMINATOR;

                    _onMessage(message);

                    return;
                }

                // command expected
                var command = data.Length < 4
                            ? string.Empty
                            : data[..4].ToUpper();

                _logger.LogDebug("Session.Process Command: => {Command}", command);

                switch (command)
                {
                    case COMMAND_QUIT:
                        networkStream.Close();
                        return;

                    case COMMAND_EHLO:
                    case COMMAND_HELO:
                        ClientId = data.Substring(4).Trim();
                        _logger.LogDebug("Session.Process ClientId: => {ClientId}", ClientId);

                        await WriteAsync(networkStream, ReplyCodes.Completed_250, string.Format(Resources.Hello_250, ClientId));

                        Status = SocketHandlerStates.Identified;
                        continue;

                    case COMMAND_NOOP:
                        await WriteAsync(networkStream, ReplyCodes.Completed_250, string.Format(Resources.Hello_250, ClientId));
                        continue;

                    case COMMAND_RSET:
                        await WriteAsync(networkStream, ReplyCodes.Completed_250, string.Format(Resources.Hello_250, ClientId));

                        Status = SocketHandlerStates.Identified;
                        continue;
                }

                if (Status < SocketHandlerStates.Identified)
                {
                    // commands beyond her need minimum of Identified state
                    await WriteAsync(networkStream, ReplyCodes.CommandSequenceError_503, Resources.ExpectedHELO_503);
                    networkStream.Close();
                    return;
                }

                switch (command)
                {
                    case COMMAND_MAIL:
                        // store the from address, in case its not in the header
                        from = data.Tail(":").ToMailAddress();
                        recipients = null;

                        _logger.LogDebug("Session.Process From: => {From}", from);

                        Status = SocketHandlerStates.Mail;
                        await WriteAsync(networkStream, ReplyCodes.Completed_250);

                        continue;

                    case COMMAND_RCPT:

                        if (Status != SocketHandlerStates.Mail
                            && Status != SocketHandlerStates.Recipient)
                        {
                            await WriteAsync(networkStream, ReplyCodes.CommandSequenceError_503);
                        }
                        else
                        {
                            if (recipients == null) recipients = new List<MailAddress>();
                            var recipient = data.Tail(":").ToMailAddress();
                            recipients.Add(recipient);

                            _logger.LogDebug("Session.Process To: => {To}", recipient);

                            Status = SocketHandlerStates.Recipient;
                            await WriteAsync(networkStream, ReplyCodes.Completed_250);
                        }

                        continue;
                    case COMMAND_DATA:

                        // request data
                        Status = SocketHandlerStates.Data;
                        await WriteAsync(networkStream, ReplyCodes.StartInput_354);

                        terminator = SMTPImpostorMessage.DATA_TERMINATOR;

                        continue;
                    default:

                        await WriteAsync(networkStream, ReplyCodes.CommandNotImplemented_502);

                        networkStream.Close();
                        return;
                }
            }
        }

        async Task WriteAsync(NetworkStream networkStream, string data)
        {
            _logger.LogDebug("Session.Write: => {Data}", data);

            data += SMTPImpostorMessage.LINE_TERMINATOR;

            var bytes = Encoding.UTF8.GetBytes(data);
            await networkStream.WriteAsync(bytes, 0, bytes.Length);
        }

        async Task WriteAsync(NetworkStream networkStream, ReplyCodes code, string description)
        {
            await WriteAsync(networkStream, $"{code:D} {description}");
        }

        async Task WriteAsync(NetworkStream networkStream, ReplyCodes code)
        {
            await WriteAsync(networkStream, code, Resources.Text[code]);
        }

    }
}
