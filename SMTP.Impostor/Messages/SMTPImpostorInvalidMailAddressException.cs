using System;
using System.Runtime.Serialization;

namespace SMTP.Impostor.Messages
{
    [Serializable]
    public class SMTPImpostorInvalidMailAddressException : Exception
    {
        public SMTPImpostorInvalidMailAddressException(
            string address)
            : base($"{address} not recognised as a valid e-mail address")
        {
            Address = address;
        }

        protected SMTPImpostorInvalidMailAddressException(
            SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        public string Address { get; }
    }
}
