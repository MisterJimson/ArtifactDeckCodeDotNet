using Newtonsoft.Json;
using System.IO;

namespace ArtifactDeckCodeDotNet.Tests
{
    /*
        GreenBlackExample

        Heroes:
        1 Lycan
        1 Rix
        1 Phantom Assassin
        1 Debbi the Cunning
        1 Chen

        Main Deck:
        3 Pick Off
        3 Payday
        3 Steam Cannon
        3 Mist of Avernus
        3 Selemene's Favor
        3 Iron Fog Goldmine
        3 Untested Grunt
        3 Thunderhide Pack
        3 Revtel Convoy

        Item Deck:
        1 Apotheosis Blade
        1 Horn of the Alpha
        3 Poaching Knife
        1 Red Mist Maul
        1 Leather Armor
        2 Traveler's Cloak
    */

    public static class TestDeckCodes
    {
        public static string GreenBlackExample => "ADCJWkTZX05uwGDCRV4XQGy3QGLmqUBg4GQJgGLGgO7AaABR3JlZW4vQmxhY2sgRXhhbXBsZQ__";
    }

    public static class TestDecks
    {
        public static Deck GreenBlackExample => JsonConvert.DeserializeObject<Deck>(File.ReadAllText("GreenBlackExample.json"));
    }
}