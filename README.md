# ArtifactDeckCodeDotNet [![NuGet](https://buildstats.info/nuget/ArtifactDeckCodeDotNet)](https://www.nuget.org/packages/ArtifactDeckCodeDotNet)
A C# port of [ArtifactDeckCode](https://github.com/ValveSoftware/ArtifactDeckCode)

Refer to that page for more detailed information on Artifact Deck Codes.
## Documentation
### Decoder
ParseDeck returns a Deck object with contains a list of Heroes, Cards, and a Name;
```csharp
Deck deck = ArtifactDeckDecoder.ParseDeck("ADCJWkTZX05uwGDCRV4XQGy3QGLmqUBg4GQJgGLGgO7AaABR3JlZW4vQmxhY2sgRXhhbXBsZQ__");
Console.WriteLine(deck.Name); // outputs "Green/Black Example"
```
### Encoder
EncodeDeck returns a string. This string is the ArtifactDeckCode.
```csharp
Deck deck = new Deck();
deck.Name = "my sweet deck";
deck.Heroes = new List<Hero>();
deck.Cards = new List<Card>();
deck.Heroes.Add(new Hero { Id = 4005, Turn = 1 });
deck.Heroes.Add(new Hero { Id = 10014, Turn = 1 });
deck.Cards.Add(new Card { Id = 3001, Count = 1 });
deck.Cards.Add(new Card { Id = 10165, Count = 3 });

...

string deckCode = ArtifactDeckEncoder.EncodeDeck(deck);
Console.WriteLine(deckCode); // outputs URL safe deck code string
```
### CardSetApiClient
The CardSetApiClient allows you to request details on all cards in Artifact by set id. Currently "00" and "01" are the only valid set ids.
```csharp
using(var apiClient = new CardSetApiClient())
{
    CardSet cardSet = await client.GetCardSetAsync(0);

    Console.WriteLine(cardSet.Version); //outputs "1"
    Console.WriteLine(cardSet.SetInfo.Name.English); //outputs "Base Set"
    Console.WriteLine(string.Join(", ", cardSet.CardList)
      .Skip(15)
      .Take(3)
      .Select(x => x.CardName.English)); //outputs "Town Portal Scroll, Fahrvhan the Dreamer, Pack Leadership"
}
```
