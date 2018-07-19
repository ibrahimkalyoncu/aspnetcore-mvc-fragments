using System.Threading.Tasks;
using AspNetCore.Mvc.Fragments.Http;
using AspNetCore.Mvc.Fragments.Options;
using Microsoft.Extensions.Caching.Memory;

namespace AspNetCore.Mvc.Fragments.Remote
{
    public class RemoteFragment : Fragment
    {
        private readonly FragmentInfo _fragmentInfo;
        private readonly IHttpClientProvider _httpClientProvider;
        private readonly IMemoryCache _memoryCache;
        public IFragmentOptions Options => _fragmentInfo?.FragmentOptions;

        public RemoteFragment(FragmentInfo fragmentInfo, IHttpClientProvider httpClientProvider, IMemoryCache memoryCache)
        {
            _fragmentInfo = fragmentInfo;
            _httpClientProvider = httpClientProvider;
            _memoryCache = memoryCache;
        }

        public override async Task<string> GetPlaceHolderHtmlAsync()
        {
            var requestUri = $"{_fragmentInfo.Source}/{_fragmentInfo.Name}/placeholder";

            if (_memoryCache.TryGetValue(requestUri, out string cachedHtml))
            {
                return cachedHtml;
            }

            var placeHolderHtml = await _httpClientProvider.GetAsStringAsync(requestUri);
            _memoryCache.Set(requestUri, placeHolderHtml);
            return placeHolderHtml;
        }

        public async Task<string> ProcessAsync(object model)
        {
            var requestUri = $"{_fragmentInfo.Source}/{_fragmentInfo.Name}/content";

            if (_memoryCache.TryGetValue(requestUri, out string cachedHtml))
            {
                return cachedHtml;
            }

            return await _httpClientProvider.PostAsStringAsync(requestUri, model);
        }
    }
}
