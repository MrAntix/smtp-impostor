using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SMTP.Impostor.Messages;

namespace SMTP.Impostor.Sockets
{

    internal class SocketHandler :
        ISMTPImpostorSocketHandler
    {
        public const int SessionReadBufferSize = 255;
        internal const string LINE_TERMINATOR = "\r\n";
        internal const string HEADERS_TERMINATOR = "\r\n\r\n";
        internal const string DATA_TERMINATOR = "\r\n.\r\n";

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

        static readonly object LOCK_OBJECT = new object();
        bool _disposed;

        ISMTPImpostorSocket _socket;
        NetworkStream _networkStream;
        byte[] _readBuffer;
        string _terminator;

        readonly Action<SMTPImpostorMessage> _onMessage;
        readonly TaskCompletionSource<bool> _taskCompletion;
        StringBuilder _data;
        MailAddress _from;
        List<MailAddress> _to;

        public SocketHandlerStates Status { get; set; }
        public IPAddress ClientAddress { get; set; }
        public string ClientId { get; set; }

        public SocketHandler(
            ISMTPImpostorSocket socket,
            Action<SMTPImpostorMessage> onMessage)
        {
            _socket = socket;
            _onMessage = onMessage;
            _taskCompletion = new TaskCompletionSource<bool>();
        }

        public Task HandleAsync()
        {
            ClientAddress = _socket.RemoteEndPoint.Address;

            _networkStream = _socket.GetNetworkStream();
            Write(
                ReplyCodes.Ready_220,
                string.Format(Resources.Ready_220, ClientAddress));

            Status = SocketHandlerStates.Connected;
            //Host.RaiseEvent(HostEventTypes.SessionConnected, this);

            Read(LINE_TERMINATOR);

            return _taskCompletion.Task;
        }

        /// <summary>
        ///   <para>Set up a read to a terminator</para>
        /// </summary>
        /// <param name = "terminator"></param>
        void Read(string terminator)
        {
            _terminator = terminator;
            _data = new StringBuilder();

            Read();
        }

        /// <summary>
        ///   <para>Sets up an async read</para>
        /// </summary>
        void Read()
        {
            if (_networkStream == null) return;

            try
            {
                _readBuffer = new byte[SessionReadBufferSize];
                _networkStream.BeginRead(
                    _readBuffer, 0, _readBuffer.Length,
                    ReadCallback,
                    this);
            }
            catch (Exception)
            {
                Dispose();
            }
        }

        /// <summary>
        ///   <para>Read the data</para>
        /// </summary>
        /// <param name = "result"></param>
        //[DebuggerStepThrough]
        void ReadCallback(IAsyncResult result)
        {
            if (_networkStream == null) return;

            try
            {
                var readData = Encoding.UTF8.GetString(
                    _readBuffer,
                    0, _networkStream.EndRead(result));
                _data.Append(readData);
                // check for terminator
                if (_data.Length > _terminator.Length
                    && _data.ToString(_data.Length - _terminator.Length, _terminator.Length).Equals(_terminator))
                {
                    // process data received not including the terminator
                    Process(
                        _data.ToString(
                            0, _data.Length - _terminator.Length));
                }
                else if (!_disposed)
                {
                    // read some more data
                    Read();
                }
            }
            catch (Exception)
            {
                Dispose();
            }
        }

        void Write(string data)
        {
            // _logger.LogInformation("Session.Write: => {0}", data);

            data += LINE_TERMINATOR;

            var bytes = Encoding.UTF8.GetBytes(data);
            _networkStream.Write(bytes, 0, bytes.Length);
        }

        void Write(ReplyCodes code, string description)
        {
            Write(String.Format("{0:D} {1}", code, description));
        }

        void Write(ReplyCodes code)
        {
            Write(code, Resources.Text[code]);
        }

        /// <summary>
        ///   <para>Process the current data read</para>
        /// </summary>
        void Process(string data)
        {
            var terminator = LINE_TERMINATOR;
            try
            {
                if (Status == SocketHandlerStates.Data)
                {
                    var content = _data.ToString(0, _data.Length - _terminator.Length);
                    var message = SMTPImpostorMessage.FromContent(content);

                    // raise event
                    _onMessage(message);

                    // _logger.LogInformation("Session.Process Data: => {0}", data);

                    Write(ReplyCodes.Completed_250);
                    Status = SocketHandlerStates.Identified;
                    _to = null;

                    Dispose();

                    return;
                }

                // command expected
                var command = data.Length < 4
                                  ? string.Empty
                                  : data.Substring(0, 4).ToUpper();
                // _logger.LogInformation("Session.Process Command: => {0}", command);

                if (command.Equals(COMMAND_QUIT))
                {
                    Dispose();
                }
                else if (command.Equals(COMMAND_EHLO)
                         || command.Equals(COMMAND_HELO))
                {
                    ClientId = data.Substring(4).Trim();
                    // _logger.LogInformation("Session.Process ClientId: => {0}", ClientId);

                    Write(ReplyCodes.Completed_250, String.Format(Resources.Hello_250, ClientId));

                    Status = SocketHandlerStates.Identified;
                    //Host.RaiseEvent(HostEventTypes.SessionIdentified, this);
                }
                else if (command.Equals(COMMAND_NOOP))
                {
                    Write(ReplyCodes.Completed_250, String.Format(Resources.Hello_250, ClientId));
                }
                else if (command.Equals(COMMAND_RSET))
                {
                    Write(ReplyCodes.Completed_250, String.Format(Resources.Hello_250, ClientId));

                    _from = null;
                    _to = null;

                    Status = SocketHandlerStates.Identified;
                    //Host.RaiseEvent(HostEventTypes.SessionIdentified, this);
                }
                else if (Status < SocketHandlerStates.Identified)
                {
                    Write(ReplyCodes.CommandSequenceError_503, Resources.ExpectedHELO_503);
                }
                else if (command.Equals(COMMAND_MAIL))
                {
                    if (Status != SocketHandlerStates.Identified)
                    {
                        Write(ReplyCodes.CommandSequenceError_503);
                    }
                    else
                    {
                        // store the from address, in case its not in the header
                        _from = data.Tail(":").ToMailAddress();
                        // _logger.LogInformation("Session.Process From: => {0}", _from);

                        Status = SocketHandlerStates.Mail;
                        Write(ReplyCodes.Completed_250);
                    }
                }
                else if (command.Equals(COMMAND_RCPT))
                {
                    if (Status != SocketHandlerStates.Mail
                        && Status != SocketHandlerStates.Recipient)
                    {
                        Write(ReplyCodes.CommandSequenceError_503);
                    }
                    else
                    {
                        if (_to == null) _to = new List<MailAddress>();
                        var to = data.Tail(":").ToMailAddress();
                        _to.Add(to);
                        // _logger.LogInformation("Session.Process To: => {0}", to);

                        Status = SocketHandlerStates.Recipient;
                        Write(ReplyCodes.Completed_250);
                    }
                }
                else if (command.Equals(COMMAND_DATA))
                {
                    // request data
                    Status = SocketHandlerStates.Data;
                    Write(ReplyCodes.StartInput_354);

                    terminator = DATA_TERMINATOR;
                }
                else
                {
                    Write(ReplyCodes.CommandNotImplemented_502);
                }
            }
            catch (SMTPImpostorInvalidMailAddressException ex)
            {
                Write(
                    ReplyCodes.ParameterError_501,
                    string.Format("Invalid Address '{0}'", ex.Address));
            }
            finally
            {
                if (!_disposed)
                {
                    // next read
                    Read(terminator);
                }
            }
        }

        public static IEnumerable<SMTPImpostorMessageHeader> GetHeaders(string data)
        {
            var headers = new List<SMTPImpostorMessageHeader>();
            if (!data.Contains(HEADERS_TERMINATOR)) return headers;

            // get the headers part of the data an un-fold 
            var rawHeaders = data.Head(HEADERS_TERMINATOR)
                .Replace(LINE_TERMINATOR + "\t", string.Empty)
                .Replace(LINE_TERMINATOR + " ", string.Empty);

            // parse into collection
            foreach (var header in rawHeaders.Split('\n'))
            {
                var value = header;
                var name = StringExtensions.Head(ref value, ":")
                    .Trim().ToLower();

                value = value.Trim();

                if (name.Length > 0)
                {
                    // add as is
                    headers.Add(new SMTPImpostorMessageHeader(
                        name,
                        SMTPImpostorDecoder.FromQuotedWord(value)
                        ));
                }
            }

            return headers;
        }

        /// <summary>
        ///   <para>Dispose</para>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        ///   <para>Finalize object</para>
        /// </summary>
        ~SocketHandler()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (LOCK_OBJECT)
            {
                if (!_disposed)
                {
                    _disposed = true;
                    if (disposing)
                    {
                        try
                        {
                            _networkStream.Dispose();

                            //Host.RaiseEvent(HostEventTypes.SessionDisconnected, this);
                        }
                        finally
                        {
                            _networkStream = null;
                            _socket = null;

                            _taskCompletion.SetResult(true);
                        }
                    }
                }
            }
        }
    }
}
