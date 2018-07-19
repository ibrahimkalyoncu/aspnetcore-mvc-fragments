using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AspNetCore.Mvc.Fragments.Http
{
    internal class HttpClientProvider : IHttpClientProvider
    {
        private readonly ConcurrentDictionary<string, HttpClient> _httpClientPool;

        public HttpClientProvider()
        {
            _httpClientPool = new ConcurrentDictionary<string, HttpClient>();
        }

        public async Task<string> GetAsStringAsync(string uri)
        {
            if (_httpClientPool.ContainsKey(uri) == false)
            {
                _httpClientPool[uri] = new HttpClient();
            }

            return await _httpClientPool[uri].GetStringAsync(uri);
        }

        public async Task<string> PostAsStringAsync(string uri, object payload)
        {
            if (_httpClientPool.ContainsKey(uri) == false)
            {
                _httpClientPool[uri] = new HttpClient();
            }

            var responseMessage = await _httpClientPool[uri].PostAsync(uri, new StringContent(JsonConvert.SerializeObject(payload), null, "application/json"));
            return await responseMessage.Content.ReadAsStringAsync();
        }
    }
}