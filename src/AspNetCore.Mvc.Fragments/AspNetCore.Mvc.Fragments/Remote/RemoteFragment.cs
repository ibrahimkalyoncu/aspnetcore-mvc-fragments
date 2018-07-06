using System.Net.Http;
using System.Threading.Tasks;
using AspNetCore.Mvc.Fragments.Options;

namespace AspNetCore.Mvc.Fragments.Remote
{
    public class RemoteFragment : Fragment
    {
        private readonly FragmentInfo _fragmentInfo;
        public IFragmentOptions Options => _fragmentInfo?.FragmentOptions;

        public RemoteFragment(FragmentInfo fragmentInfo)
        {
            _fragmentInfo = fragmentInfo;
        }

        public override async Task<string> GetPlaceHolderHtmlAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                var httpResponseMessage = await client.GetAsync($"{_fragmentInfo.Source}/{_fragmentInfo.Name}/placeholder");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return await httpResponseMessage.Content.ReadAsStringAsync();
                }
            }

            return await base.GetPlaceHolderHtmlAsync();
        }

        public async Task<string> ProcessAsync(object model)
        {
            using (HttpClient client = new HttpClient())
            {
                var httpResponseMessage = await client.GetAsync($"{_fragmentInfo.Source}/{_fragmentInfo.Name}/content");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return await httpResponseMessage.Content.ReadAsStringAsync();
                }
            }

            return string.Empty;
        }
    }
}
