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
        Lazy<Cache<uint, CardSetData>> _cache;

        private static string DataCacheFile = Path.Combine(Path.GetTempPath(), "ArtifactSetDataCache.json");

        public CardSetApiClient() : this(new HttpClient())
        {
        }

        public CardSetApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _cache = new Lazy<Cache<uint, CardSetData>>(() => new Cache<uint, CardSetData>(DataCacheFile));
        }

        public async Task<CardSet> GetCardSetAsync(uint setId, bool forceFetch = false)
        {
            Cache<uint, CardSetData> dataCache = _cache.Value;
            if (forceFetch ||
                !dataCache.TryGetValue(setId, out var data) ||
                data.ExpireTimeUtc < DateTimeOffset.UtcNow)
            {
                data = await FetchCardSetData(setId);
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

        private static Uri CardSetInfoUrl(uint setId) => new Uri($"https://playartifact.com/cardset/{setId:D2}");

        private async Task<CardSetData> FetchCardSetData(uint setId)
        {
            CardSetJsonLocation jsonLocation = await FetchCardSetLocation(CardSetInfoUrl(setId));
            CardSetData cardSetData = await FetchCardSetData(jsonLocation.GetFullUri());

            // Record set data expiration date
            cardSetData.ExpireTimeUtc = DateTimeOffset.FromUnixTimeSeconds(jsonLocation.ExpireTime);

            return cardSetData;
        }

        private async Task<CardSetData> FetchCardSetData(Uri cardSetDataUri)
        {
            try
            {
                HttpResponseMessage getCardSetResult = await _httpClient.GetAsync(cardSetDataUri);
                return JsonConvert.DeserializeObject<CardSetData>(await getCardSetResult.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                throw new CardSetApiClientException($"Failed to fetch set data from {cardSetDataUri}", ex);
            }
        }

        private async Task<CardSetJsonLocation> FetchCardSetLocation(Uri setInfoUri)
        {
            try
            {
                HttpResponseMessage getJsonLocationResult = await _httpClient.GetAsync(setInfoUri);
                return JsonConvert.DeserializeObject<CardSetJsonLocation>(await getJsonLocationResult.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                throw new CardSetApiClientException($"Failed to fetch set information from ${setInfoUri}", ex);
            }
        }

        private class Cache<TKey, TValue> : IDisposable
        {
            private readonly string _cacheFilePath;
            private readonly Dictionary<TKey, TValue> _data;

            public Cache(string filePath)
            {
                _cacheFilePath = filePath;
                _data = ReadDataFromDisk(filePath);
            }

            public TValue this[TKey key]
            {
                get => _data[key];
                set => _data[key] = value;
            }

            public bool TryGetValue(TKey key, out TValue value)
                => _data.TryGetValue(key, out value);

            public bool Remove(TKey key) => _data.Remove(key);

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

            private static Dictionary<TKey, TValue> ReadDataFromDisk(string filePath)
            {
                try
                {
                    JsonSerializer serializer = new JsonSerializer();
                    var fileContents = File.ReadAllText(filePath);
                    return JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(fileContents);
                }
                catch
                {
                    return new Dictionary<TKey, TValue>();
                }
            }
        }
    }
}