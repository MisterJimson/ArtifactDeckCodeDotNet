using System.Threading.Tasks;
using Xunit;

namespace ArtifactDeckCodeDotNet.Tests
{
    public class CardSetApiClientTests
    {
        [Fact]
        public async Task GetCardSetAsyncShouldReturnCardSet()
        {
            // Act
            CardSetApiClient client = new CardSetApiClient();
            CardSetData result = await client.GetCardSetAsync("00");

            // Verify
            Assert.Equal(1, result.CardSet.Version);
            Assert.Equal("Base Set", result.CardSet.SetInfo.Name.English);
        }
    }
}