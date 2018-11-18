using System.Collections.Generic;
using Xunit;

namespace ArtifactDeckCodeDotNet.Tests
{
    public class DecodeEncodeTests
    {
        private static List<string> DeckStrings = new List<string>()
        {
            "ADCIAAA", // completely empty deck
            "ADCJXgAZX05uwGDCRU_", // deck with nothing but 5 heroes
            "ADCKDkAAWV9ObsBg0cCQ0ER", // deck with nothing but 8 heroes
            "ADCJWw69rgCBBHPBYhBvV193QFfFIgKgV4FlwQWAzEBQweJZwKIAtCa0YDRg9GC0LDRjyDQutC+0LvQvtC00LAg0LrQsNGA0YIg0LjQtyDQmNC90YLQtdGA0L3QtdGC0LA_",
        };

        [Fact]
        public void ArtifactDecksCanBeDecodedAndEncoded()
        {
            foreach (var inputDeckString in DeckStrings)
            {
                Deck deck = ArtifactDeckDecoder.DecodeDeck(inputDeckString);
                string deckString = ArtifactDeckEncoder.EncodeDeck(deck);

                Assert.Equal(inputDeckString, deckString);
            }
        }
    }
}
