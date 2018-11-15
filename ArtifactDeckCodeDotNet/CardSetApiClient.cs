using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ArtifactDeckCodeDotNet
{
    public class CardSetApiClient : IDisposable
    {
        HttpClient _httpClient;
        Lazy<CardSetDataCache> _cache;

        internal static string DataCacheFile = Path.Combine(Path.GetTempPath(), "ArtifactSetDataCache.json");

        public CardSetApiClient() : this(new HttpClient())
        {
        }

        public CardSetApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _cache = new Lazy<CardSetDataCache>(() => new CardSetDataCache(DataCacheFile));
        }

        public async Task<CardSet> GetCardSetAsync(string setId, bool forceFetch = false)
        {
            CardSetDataCache dataCache = _cache.Value;
            if (forceFetch ||
                !dataCache.TryGetValue(setId, out var data) ||
                data.ExpireTimeUtc < DateTimeOffset.Now)
            {
                data = await FetchCardSetAsync(setId);
                dataCache[setId] = data;
            }

            return data.CardSet;
        }

        public void Dispose()
        {
            if (_cache.IsValueCreated)
            {
                _cache.Value.Dispose();
            }
        }

        private static string CardSetInfoUrl(string setId) => $"https://playartifact.com/cardset/{setId}/";

        private async Task<CardSetData> FetchCardSetAsync(string setId)
        {
            try
            {
                HttpResponseMessage getJsonLocationResult = await _httpClient.GetAsync(CardSetInfoUrl(setId));
                CardSetJsonLocation jsonLocation = JsonConvert.DeserializeObject<CardSetJsonLocation>(await getJsonLocationResult.Content.ReadAsStringAsync());

                HttpResponseMessage getCardSetResult = await _httpClient.GetAsync(jsonLocation.GetFullUri());
                CardSetData cardSetData = JsonConvert.DeserializeObject<CardSetData>(await getCardSetResult.Content.ReadAsStringAsync());

                // Record set data expiration date
                cardSetData.ExpireTimeUtc = DateTimeOffset.FromUnixTimeSeconds(jsonLocation.ExpireTime);

                return cardSetData;
            }
            catch (Exception ex)
            {
                throw new CardSetApiClientException($"Failed to fetch data for set {setId}.", ex);
            }
        }

        private class CardSetDataCache : IDisposable
        {
            private readonly string _cacheFilePath;
            private readonly Dictionary<string, CardSetData> _data;

            public ICollection<string> Keys { get; }
            public ICollection<CardSetData> Values { get; }
            public int Count { get; }
            public bool IsReadOnly { get; }

            public CardSetDataCache(string filePath)
            {
                _cacheFilePath = filePath;
                _data = ReadDataFromDisk(filePath);
            }

            public CardSetData this[string setId]
            {
                get => _data[setId];
                set => _data[setId] = value;
            }

            public bool TryGetValue(string key, out CardSetData value)
                => _data.TryGetValue(key, out value);

            public bool Remove(string key) => _data.Remove(key);

            public void Clear() => _data.Clear();

            public void Dispose()
            {
                WriteDataToDisk();
            }

            private void WriteDataToDisk()
            {
                try
                {
                    File.WriteAllText(_cacheFilePath, JsonConvert.SerializeObject(_data));
                }
                catch (IOException) { }
            }

            private static Dictionary<string, CardSetData> ReadDataFromDisk(string filePath)
            {
                try
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var fileContents = File.ReadAllText(filePath);
                    return JsonConvert.DeserializeObject<Dictionary<string, CardSetData>>(fileContents);
                }
                catch
                {
                    return new Dictionary<string, CardSetData>();
                }
            }
        }
    }
}