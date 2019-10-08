using System.IO;
using System.Text;

namespace SMTP.Impostor
{
    internal static class StreamExtensions
    {
        /// <summary>
        ///   <para>Read a stream until the first instance of a stream is found</para>
        /// </summary>
        /// <param name = "stream">A stream</param>
        /// <param name = "to">String to search for</param>
        /// <param name = "encoding">Encoding</param>
        /// <returns>Data read as a string</returns>
        public static string ReadTo(this Stream stream, string to, Encoding encoding)
        {
            var data = new StringBuilder();
            var buffer = new byte[1024];
            var toBytes = encoding.GetBytes(to);
            var matchStart = 0;
            var matchLength = 0;
            var readCount = 0;

            while (
                (readCount = stream.Read(buffer, 0, buffer.Length)) > 0
                && matchLength < toBytes.Length)
            {
                data.Append(encoding.GetString(buffer));

                for (var i = 0; i < readCount; i++)
                {
                    if (toBytes[matchLength] == buffer[i])
                    {
                        matchLength++;
                        if (matchLength == toBytes.Length)
                            return data.ToString(0, matchStart);

                        //                        if (matchLength == toBytes.Length) break;
                    }
                    else
                    {
                        matchStart++;
                        matchLength = 0;
                    }
                }
            }

            return data.ToString(0, matchStart);
        }
    }
}
