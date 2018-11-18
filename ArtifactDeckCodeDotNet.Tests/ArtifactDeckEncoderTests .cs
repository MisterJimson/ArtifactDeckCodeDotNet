using System;
using Xunit;

namespace ArtifactDeckCodeDotNet.Tests
{
    public class ArtifactDeckEncoderTests
    {
        [Fact]
        public void EncodeDeckCorrectlyEncodesDeck()
        {
            string deckCode = ArtifactDeckEncoder.EncodeDeck(TestDecks.GreenBlackExample);
            Assert.Equal(TestDeckCodes.GreenBlackExample, deckCode);
        }

        [Fact]
        public void EncodeDeckThrowsWithNullInput()
        {
            Assert.Throws<ArgumentNullException>(() => ArtifactDeckEncoder.EncodeDeck(null));
        }

        [Fact]
        public void EncodeDeckThrowsForHeroRefWithZeroTurn()
        {
            Deck deck = new Deck()
            {
                Heroes = { new HeroRef(10024, 0) }
            };

            Assert.Throws<ArtifactDeckEncoderException>(() => ArtifactDeckEncoder.EncodeDeck(deck));
        }

        [Fact]
        public void EncodeDeckAcceptsHeroRefWithZeroId()
        {
            Deck deck = new Deck()
            {
                Heroes = { new HeroRef(0, 1) }
            };

            // This should probably be an error, but for some reason it's not in Valve's reference implementation.
            string deckCode = ArtifactDeckEncoder.EncodeDeck(deck);
            Assert.NotNull(deckCode);
        }

        [Fact]
        public void EncodeDeckThrowsForCardRefWithZeroCount()
        {
            Deck deck = new Deck()
            {
                Cards = { new CardRef(3000, 0) }
            };

            Assert.Throws<ArtifactDeckEncoderException>(() => ArtifactDeckEncoder.EncodeDeck(deck));
        }

        [Fact]
        public void EncodeDeckThrowsForCardRefWithZeroId()
        {
            Deck deck = new Deck()
            {
                Cards = { new CardRef(0, 2) }
            };

            Assert.Throws<ArtifactDeckEncoderException>(() => ArtifactDeckEncoder.EncodeDeck(deck));
        }

        [Fact]
        public void EncodeDeckTruncatesLongDeckNames()
        {
            Deck deckWithLongName = new Deck() { Name = "이것은 전 세계와 아마도 우주에서 최고의 카드 덱입니다" };
            Deck deckWithTruncatedName = new Deck() { Name = "이것은 전 세계와 아마도 우주에서 최고의 카드" };

            Assert.Equal(ArtifactDeckEncoder.EncodeDeck(deckWithTruncatedName), ArtifactDeckEncoder.EncodeDeck(deckWithLongName));
        }
    }
}
