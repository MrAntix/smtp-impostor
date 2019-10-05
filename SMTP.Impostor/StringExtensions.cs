using System;

namespace SMTP.Impostor
{
    internal static class StringExtensions
    {
        public static string Head(ref string text, string uptoItem, StringComparison comparisonType)
        {
            var position = text.IndexOf(uptoItem, comparisonType);
            string headText;

            if (position == -1)
            {
                headText = text;
                text = "";
            }
            else
            {
                headText = text.Substring(0, position);
                text = text.Substring(position + uptoItem.Length);
            }

            return headText;
        }

        public static string Head(ref string text, string uptoItem)
        {
            return Head(ref text, uptoItem, StringComparison.CurrentCulture);
        }

        public static string Head(this string text, string uptoItem, StringComparison comparisonType)
        {
            var position = text.IndexOf(uptoItem, comparisonType);
            string headText;

            if (position == -1)
            {
                headText = text;
            }
            else
            {
                headText = text.Substring(0, position);
            }

            return headText;
        }

        public static string Head(this string text, string uptoItem)
        {
            return Head(text, uptoItem, StringComparison.CurrentCulture);
        }

        public static string Tail(ref string text, string uptoItem, StringComparison comparisonType)
        {
            var position = text.LastIndexOf(uptoItem, comparisonType);
            string tailText;

            if (position == -1)
            {
                tailText = text;
                text = "";
            }
            else
            {
                tailText = text.Substring(position + uptoItem.Length);
                text = text.Substring(0, position);
            }

            return tailText;
        }

        public static string Tail(ref string text, string uptoItem)
        {
            return Tail(ref text, uptoItem, StringComparison.CurrentCulture);
        }

        public static string Tail(this string text, string uptoItem, StringComparison comparisonType)
        {
            var position = text.LastIndexOf(uptoItem, comparisonType);
            string tailText;

            if (position == -1)
            {
                tailText = text;
            }
            else
            {
                tailText = text.Substring(position + uptoItem.Length);
            }

            return tailText;
        }

        public static string Tail(this string text, string uptoItem)
        {
            return Tail(text, uptoItem, StringComparison.CurrentCulture);
        }
    }
}
