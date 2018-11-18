using System.Text;
using Xunit;

namespace ArtifactDeckCodeDotNet.Tests
{
    public class UtilsTests
    {
        [Fact]
        public void SanitizeRemovesSimpleHtmlTags()
        {
            Assert.Equal("Simple string", Utils.Sanitize("Simple string"));
            Assert.Equal("String with simple HTML", Utils.Sanitize("<p>String <b>with</b> simple HTML</p>"));
            Assert.Equal("String with embeded link", Utils.Sanitize("String with <a href=\"cat.png\">embeded link</a>"));
        }

        [Fact]
        public void PrefixWithMaxBytesWorksWithEnglishText()
        {
            Assert.Equal("abcd", Utils.PrefixWithMaxBytes("abcd", 10, Encoding.UTF8));
            Assert.Equal("abcde", Utils.PrefixWithMaxBytes("abcdefgh", 5, Encoding.UTF8));
            Assert.Equal("A", Utils.PrefixWithMaxBytes("AB", 1, Encoding.UTF8));
        }

        [Fact]
        public void PrefixWithMaxBytesWorksWithRussianText()
        {
            Assert.Equal("молитва", Utils.PrefixWithMaxBytes("молитва", 14, Encoding.UTF8));
            Assert.Equal("Сто", Utils.PrefixWithMaxBytes("Сторожевая башня", 7, Encoding.UTF8));
            Assert.Equal(string.Empty, Utils.PrefixWithMaxBytes("урон", 1, Encoding.UTF8));
        }

        [Fact]
        public void PrefixWithMaxBytesWorksWithKoreanText()
        {
            Assert.Equal("축복", Utils.PrefixWithMaxBytes("축복", 6, Encoding.UTF8));
            Assert.Equal("루무", Utils.PrefixWithMaxBytes("루무스크의", 7, Encoding.UTF8));
            Assert.Equal(string.Empty, Utils.PrefixWithMaxBytes("在", 2, Encoding.UTF8));
        }

        [Fact]
        public void FullChecksumWorksOnArray()
        {
            byte[] data = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Assert.Equal(15u, Utils.FullChecksum(data, 0, 5));
            Assert.Equal(0u, Utils.FullChecksum(data, data.Length, 0));
        }
    }
}