# ArtifactDeckCodeDotNet [![NuGet](https://buildstats.info/nuget/ArtifactDeckCodeDotNet)](https://www.nuget.org/packages/ArtifactDeckCodeDotNet)
A C# port of [ArtifactDeckCode](https://github.com/ValveSoftware/ArtifactDeckCode)
## Documentation
### Decoder
ParseDeck returns a Deck object with contains a list of Heroes, Cards, and a Name;
```csharp
Deck deck = ArtifactDeckDecoder.ParseDeck("ADCJWkTZX05uwGDCRV4XQGy3QGLmqUBg4GQJgGLGgO7AaABR3JlZW4vQmxhY2sgRXhhbXBsZQ__");
Console.WriteLine(deck.Name); // outputs "Green/Black Example"
```
