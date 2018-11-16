using System.Threading.Tasks;
using Xunit;

namespace ArtifactDeckCodeDotNet.Tests
{
    public class CardSetApiClientTests
    {
        [Fact]
        public async Task GetCardSetAsyncShouldReturnDataForSet00()
        {
            using (CardSetApiClient client = new CardSetApiClient())
            {
                CardSet result = await client.GetCardSetAsync(0);

                Assert.Equal(1, result.Version);
                Assert.Equal(0, result.SetInfo.SetId);
                Assert.Equal("Base Set", result.SetInfo.Name.English);
                Assert.True(result.CardList.Count > 0);
            }
        }

        [Fact]
        public async Task GetCardSetAsyncShouldReturnDataForSet01()
        {
            using (CardSetApiClient client = new CardSetApiClient())
            {
                CardSet result = await client.GetCardSetAsync(1);

                Assert.Equal(1, result.Version);
                Assert.Equal(1, result.SetInfo.SetId);
                Assert.Equal("Call to Arms", result.SetInfo.Name.English);
                Assert.True(result.CardList.Count > 0);
            }
        }

        [Fact]
        public async Task GetCardSetAsyncShouldThrowForInvalidSet()
        {
            CardSetApiClient client = new CardSetApiClient();

            await Assert.ThrowsAsync<CardSetApiClientException>(() => client.GetCardSetAsync(2));
        }
    }
}