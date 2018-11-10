using System.Collections.Generic;

namespace ArtifactDeckCodeDotNet
{
    public class Hero
    {
        public int Id { get; set; } 
        public int Turn { get; set; }
    }

    public class Card
    {
        public int Id { get; set; }
        public int Count { get; set; }
    }

    public class Deck
    {
        public string Name { get; set; }
        public List<Hero> Heroes { get; set; }
        public List<Card> Cards { get; set; }
    }
}
