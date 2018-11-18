using System;
using System.Linq;
using Xunit;

namespace ArtifactDeckCodeDotNet.Tests
{
    public class ArtifactDeckDecoderTests
    {
        [Fact]
        public void ParseGreenBlackExampleDeckShouldCorrectlyParseDeck()
        {
            // Act
            Deck deck = ArtifactDeckDecoder.DecodeDeck(TestDeckCodes.GreenBlackExample);

            // Verify
            Assert.Equal(5, deck.Heroes.Count);
            Assert.Equal(15, deck.Cards.Count);
            Assert.Equal(51, (deck.Heroes.Count * 3) + deck.Cards.Sum(c => c.Count));
            Assert.Equal("Green/Black Example", deck.Name);

            Assert.Contains(deck.Heroes, x => x.Id == 10014); // Lycan
            Assert.Contains(deck.Heroes, x => x.Id == 10026); // Rix
            Assert.Contains(deck.Heroes, x => x.Id == 10047); // Phantom Assassin
            Assert.Contains(deck.Heroes, x => x.Id == 4005);  // Debbi the Cunning
            Assert.Contains(deck.Heroes, x => x.Id == 10017); // Chen

            Assert.Contains(deck.Cards, x => x.Id == 3000 && x.Count == 2);  // Traveler's Cloak
            Assert.Contains(deck.Cards, x => x.Id == 10102 && x.Count == 3); // Thunderhide Pack
        }

        [Fact]
        public void ParseBlueRedExampleDeckShouldCorrectlyParseDeck()
        {
            // Act
            Deck deck = ArtifactDeckDecoder.DecodeDeck(TestDeckCodes.BlueRedExample);

            // Verify
            Assert.Equal(5, deck.Heroes.Count);
            Assert.Equal(15, deck.Cards.Count);
            Assert.Equal(53, (deck.Heroes.Count * 3) + deck.Cards.Sum(c => c.Count));
            Assert.Equal("Blue/Red Example", deck.Name);

            Assert.Contains(deck.Heroes, x => x.Id == 4003);  // Keefe the Bold
            Assert.Contains(deck.Heroes, x => x.Id == 10006); // Luna
            Assert.Contains(deck.Heroes, x => x.Id == 10030); // Bristleback
            Assert.Contains(deck.Heroes, x => x.Id == 10065); // Zeus
            Assert.Contains(deck.Heroes, x => x.Id == 10033); // Earthshaker

            Assert.Contains(deck.Cards, x => x.Id == 10191 && x.Count == 2); // Routed
            Assert.Contains(deck.Cards, x => x.Id == 10411 && x.Count == 3); // Cunning Plan
        }

        [Fact]
        public void DecodeDeckThrowsWithNullInput()
        {
            Assert.Throws<ArgumentNullException>(() => ArtifactDeckDecoder.DecodeDeck(null));
        }
    }
}