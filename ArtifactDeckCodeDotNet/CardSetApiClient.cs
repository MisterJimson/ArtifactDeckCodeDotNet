using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ArtifactDeckCodeDotNet
{
    public class CardSetApiClient
    {
        HttpClient _httpClient;
        CardSetJsonLocation _jsonLocation;

        public CardSetApiClient()
        {
            _httpClient = new HttpClient();
        }

        public CardSetApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CardSetData> GetCardSetAsync(string setId)
        {
            await GetJsonLocation(setId);

            HttpResponseMessage getCardSetResult = await _httpClient.GetAsync(_jsonLocation.CdnRoot + _jsonLocation.Url);
            CardSetData cardSetData = JsonConvert.DeserializeObject<CardSetData>(await getCardSetResult.Content.ReadAsStringAsync());
            return cardSetData;
        }

        private async Task GetJsonLocation(string setId)
        {
            if (_jsonLocation == null || _jsonLocation.ExpireTime < UnixTimeNow())
            {
                HttpResponseMessage getJsonLocationResult = await _httpClient.GetAsync($"https://playartifact.com/cardset/{setId}/");
                _jsonLocation = JsonConvert.DeserializeObject<CardSetJsonLocation>(await getJsonLocationResult.Content.ReadAsStringAsync());
            }
        }

        public long UnixTimeNow()
        {
            TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            return (long)timeSpan.TotalSeconds;
        }
    }
}