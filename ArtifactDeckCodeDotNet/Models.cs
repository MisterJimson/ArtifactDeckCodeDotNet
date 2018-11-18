using System.Collections.Generic;

namespace ArtifactDeckCodeDotNet
{
    public interface ICard
    {
        uint Id { get; set; }
    }

    public struct HeroRef : ICard
    {
        public uint Id { get; set; }
        public uint Turn { get; set; }

        public HeroRef(uint id, uint turn)
        {
            Id = id;
            Turn = turn;
        }
    }

    public struct CardRef : ICard
    {
        public uint Id { get; set; }
        public uint Count { get; set; }

        public CardRef(uint id, uint count)
        {
            Id = id;
            Count = count;
        }
    }

    public class Deck
    {
        public string Name { get; set; } = string.Empty;
        public List<HeroRef> Heroes { get; } = new List<HeroRef>();
        public List<CardRef> Cards { get; } = new List<CardRef>();
    }
}
