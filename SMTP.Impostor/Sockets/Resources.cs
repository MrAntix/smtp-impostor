using System.Collections.Generic;

namespace SMTP.Impostor.Sockets
{
    internal static class Resources
    {
        public static string GetText(ReplyCodes code)
        {
            return Text.ContainsKey(code) ? Text[code] : code.ToString();
        }

        public static IDictionary<ReplyCodes, string> Text = new Dictionary<ReplyCodes, string>
        {
            {ReplyCodes.Ready_220 , Ready_220},
            {ReplyCodes.Completed_250 , Completed_250},
            {ReplyCodes.StartInput_354 , StartInput_354},
            {ReplyCodes.SyntaxError_500 , SyntaxError_500},
            {ReplyCodes.ParameterError_501 , ParameterError_501},
            {ReplyCodes.CommandNotImplemented_502 , CommandNotImplemented_502},
            {ReplyCodes.CommandSequenceError_503 , ExpectedHELO_503},
            {ReplyCodes.CommandParameterNotImplemented_504 , StartInput_354},
        };

        public const string Ready_220 = "Welcome {0}, Antix SMTP Impostor";
        public const string Completed_250 = "OK";
        public const string Goodbye_250 = "Good bye";
        public const string Hello_250 = "Hello {0}";
        public const string StartInput_354 = "Send Data, end with &lt; CRLF&gt;.&lt;CRLF&gt;";
        public const string SyntaxError_500 = "Syntax Error";
        public const string ParameterError_501 = "Parameter Error";
        public const string CommandNotImplemented_502 = "Command out of sequence";
        public const string ExpectedHELO_503 = "Expected HELO &lt; Your Name&gt;";
        public const string CommandParameterNotImplemented_504 = "Command Parameter Not Implemented";
    }
}
