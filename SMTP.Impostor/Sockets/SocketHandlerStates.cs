namespace SMTP.Impostor.Sockets
{
    internal enum SocketHandlerStates
    {
        /// <summary>
        ///   SessionConnected to client
        /// </summary>
        Connected,

        /// <summary>
        ///   Client has been identified
        /// </summary>
        Identified,

        /// <summary>
        ///   Receiving e-mail data
        /// </summary>
        Mail,

        /// <summary>
        ///   Receiving recipient data
        /// </summary>
        Recipient,

        /// <summary>
        ///   Receiving payload data
        /// </summary>
        Data,

        /// <summary>
        ///   SessionDisconnected from client
        /// </summary>
        Disconnected
    }
}
