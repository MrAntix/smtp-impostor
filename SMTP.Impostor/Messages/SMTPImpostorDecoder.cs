using System;
using System.Text;

namespace SMTP.Impostor.Messages
{
    // https://github.com/petrohi/Stratosphere.Imap/blob/master/Stratosphere/Imap/RFC2047Decoder.cs
    public static class SMTPImpostorDecoder
    {
        public static string FromQuotedWord(string input)
        {
            var sb = new StringBuilder();

            if (!input.StartsWith("=?"))
                return input;

            if (!input.EndsWith("?="))
                return input;

            // Get the name of the encoding but skip the leading =?
            var encodingName = input.Substring(2, input.IndexOf("?", 2) - 2);
            var enc = Encoding.ASCII;
            if (!string.IsNullOrEmpty(encodingName))
            {
                enc = Encoding.GetEncoding(encodingName);
            }

            // Get the type of the encoding
            var type = input[encodingName.Length + 3];

            // Start after the name of the encoding and the other required parts
            var startPosition = encodingName.Length + 5;

            switch (char.ToLowerInvariant(type))
            {
                case 'q':
                    sb.Append(FromQuotedPrintable(input, enc, startPosition, true));
                    break;
                case 'b':
                    var baseString = input.Substring(startPosition, input.Length - startPosition - 2);
                    var baseDecoded = Convert.FromBase64String(baseString);
                    var intermediate = enc.GetString(baseDecoded);
                    sb.Append(intermediate);
                    break;
            }
            return sb.ToString();
        }

        public static string FromQuotedPrintable(string input, Encoding enc, int startPos = 0, bool skipQuestionEquals = false)
        {
            var workingBytes = Encoding.ASCII.GetBytes(input);

            var i = startPos;
            var outputPos = i;

            while (i < workingBytes.Length)
            {
                var currentByte = workingBytes[i];
                var peekAhead = new char[2];
                switch (currentByte)
                {
                    case (byte)'=':
                        var canPeekAhead = (i < workingBytes.Length - 2);

                        if (!canPeekAhead)
                        {
                            workingBytes[outputPos] = workingBytes[i];
                            ++outputPos;
                            ++i;
                            break;
                        }

                        var skipNewLineCount = 0;
                        for (var j = 0; j < 2; ++j)
                        {
                            var c = (char)workingBytes[i + j + 1];
                            if ('\r' == c || '\n' == c)
                            {
                                ++skipNewLineCount;
                            }
                        }

                        if (skipNewLineCount > 0)
                        {
                            // If we have a lone equals followed by newline chars, then this is an artificial
                            // line break that should be skipped past.
                            i += 1 + skipNewLineCount;
                        }
                        else
                        {
                            try
                            {
                                peekAhead[0] = (char)workingBytes[i + 1];
                                peekAhead[1] = (char)workingBytes[i + 2];

                                var decodedByte = Convert.ToByte(new string(peekAhead, 0, 2), 16);
                                workingBytes[outputPos] = decodedByte;

                                ++outputPos;
                                i += 3;
                            }
                            catch (Exception)
                            {
                                // could not parse the peek-ahead chars as a hex number... so gobble the un-encoded '='
                                i += 1;
                            }
                        }
                        break;

                    case (byte)'?':
                        if (skipQuestionEquals && workingBytes[i + 1] == (byte)'=')
                        {
                            i += 2;
                        }
                        else
                        {
                            workingBytes[outputPos] = workingBytes[i];
                            ++outputPos;
                            ++i;
                        }
                        break;

                    default:
                        workingBytes[outputPos] = workingBytes[i];
                        ++outputPos;
                        ++i;
                        break;
                }
            }

            var output = string.Empty;

            var numBytes = outputPos - startPos;
            if (numBytes > 0)
            {
                output = enc.GetString(workingBytes, startPos, numBytes);
            }

            return output;
        }
    }
}
