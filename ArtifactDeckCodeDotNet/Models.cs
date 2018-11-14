using System.Collections.Generic;

namespace ArtifactDeckCodeDotNet
{
    public interface ICard
    {
        int Id { get; set; }
    }

    public class Hero : ICard
    {
        public int Id { get; set; }
        public int Turn { get; set; }
    }

    public class Card : ICard
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
