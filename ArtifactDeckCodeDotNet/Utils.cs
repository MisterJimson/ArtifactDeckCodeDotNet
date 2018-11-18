using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("ArtifactDeckCodeDotNet.Tests")]

namespace ArtifactDeckCodeDotNet
{
    internal static class Utils
    {
        /// <summary>
        /// Computes the longest string prefix that can be represented in at most maxByteLength
        /// in the specified encoding.
        /// </summary>
        public static string PrefixWithMaxBytes(string str, int maxByteLength, Encoding encoding)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            else if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));
            else if (maxByteLength < 0)
                throw new ArgumentException($"The {nameof(maxByteLength)} must be greater than zero.");
            else if (str == string.Empty || encoding.GetByteCount(str) <= maxByteLength)
                return str;

            var sb = new StringBuilder(maxByteLength);
            int byteCount = 0;
            var enumerator = StringInfo.GetTextElementEnumerator(str);
            while (enumerator.MoveNext())
            {
                string textElement = enumerator.GetTextElement();
                byteCount += encoding.GetByteCount(textElement);
                if (byteCount <= maxByteLength)
                {
                    sb.Append(textElement);
                }
                else
                {
                    break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Removes HTML tags from the specified string.
        /// This is a very simplistic implementation, but it's better than nothing.
        /// </summary>
        public static string Sanitize(string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            return Regex.Replace(str, "<.*?>", string.Empty);
        }

        /// <summary>
        /// Computes a checksum of the sub-list starting at startIndex and with the specified length.
        /// </summary>
        public static uint FullChecksum(IList<byte> bytes, int startIndex, int length)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            else if (startIndex < 0 || startIndex > bytes.Count)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            else if (length < 0 || startIndex + length > bytes.Count)
                throw new ArgumentOutOfRangeException(nameof(length));

            uint sum = 0;
            int endIndex = startIndex + length - 1;
            for (int i = startIndex; i <= endIndex; ++i)
            {
                sum += bytes[i];
            }

            return sum;
        }

        public static byte ShortChecksum(IList<byte> bytes, int startIndex, int length)
        {
            return (byte)(0xFF & FullChecksum(bytes, startIndex, length));
        }
    }
}
