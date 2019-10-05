namespace SMTP.Impostor.Messages
{
    public class SMTPImpostorMessageHeader
    {
        public const string CONTENT_TYPE = "content-type";
        public const string PART_CHAR_SET = "charset";

        public const string MESSAGE_ID = "_message-id";
        public const string FROM = "from";
        public const string DATE = "date";
        public const string TO = "to";
        public const string CC = "cc";
        public const string SUBJECT = "subject";

        public SMTPImpostorMessageHeader(
                string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }

        public override string ToString()
        {
            return $"{Name}:{Value}";
        }
    }
}
