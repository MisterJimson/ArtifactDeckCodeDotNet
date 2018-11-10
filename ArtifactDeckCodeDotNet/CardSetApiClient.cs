using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ArtifactDeckCodeDotNet
{
    public class CardSetApiClient
    {
        HttpClient _httpClient;
        Dictionary<string, CardSetJsonLocation> _jsonLocations = new Dictionary<string, CardSetJsonLocation>();
        Dictionary<string, CardSetData> _cardSetData = new Dictionary<string, CardSetData>();

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

            if (!_cardSetData.ContainsKey(setId))
            {
                HttpResponseMessage getCardSetResult = await _httpClient.GetAsync(_jsonLocations[setId].CdnRoot + _jsonLocations[setId].Url);
                CardSetData cardSetData = JsonConvert.DeserializeObject<CardSetData>(await getCardSetResult.Content.ReadAsStringAsync());
                _cardSetData.Add(setId, cardSetData);
            }
            
            return _cardSetData[setId];
        }

        private async Task GetJsonLocation(string setId)
        {
            bool containsKey = _jsonLocations.ContainsKey(setId);
            if (!containsKey || _jsonLocations[setId].ExpireTime < UnixTimeNow())
            {
                HttpResponseMessage getJsonLocationResult = await _httpClient.GetAsync($"https://playartifact.com/cardset/{setId}/");
                CardSetJsonLocation jsonLocation = JsonConvert.DeserializeObject<CardSetJsonLocation>(await getJsonLocationResult.Content.ReadAsStringAsync());

                if (containsKey)
                {
                    _jsonLocations.Remove(setId);

                    if (_cardSetData.ContainsKey(setId))
                        _cardSetData.Remove(setId);
                }

                _jsonLocations.Add(setId, jsonLocation);
            }
        }

        public long UnixTimeNow()
        {
            TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            return (long)timeSpan.TotalSeconds;
        }
    }
}