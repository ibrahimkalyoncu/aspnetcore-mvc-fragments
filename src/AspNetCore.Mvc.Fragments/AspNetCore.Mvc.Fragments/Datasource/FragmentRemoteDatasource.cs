using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AspNetCore.Mvc.Fragments.Converters;
using Newtonsoft.Json;

namespace AspNetCore.Mvc.Fragments.Datasource
{
    public class FragmentRemoteDatasource : IFragmentDatasource
    {
        private readonly string _fragmentSourceUrl;
        private List<FragmentInfo> _fragments;

        public FragmentRemoteDatasource(string fragmentSourceUrl)
        {
            _fragmentSourceUrl = fragmentSourceUrl;
            _fragments = null;
        }

        public async Task<List<FragmentInfo>> GetAllAsync()
        {
            if (_fragments == null)
            {
                await LoadFragmentsAsync();
            }

            return _fragments;
        }

        private async Task LoadFragmentsAsync()
        {
            using (var client = new HttpClient())
            {
                var responseContent = await (await client.GetAsync(_fragmentSourceUrl)).Content.ReadAsStringAsync();
                _fragments = JsonConvert.DeserializeObject<List<FragmentInfo>>(responseContent, new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter> { new FragmentOptionsConverter() }
                });

                _fragments?.ForEach(f =>
                {
                    NormalizeUrls(f.FragmentOptions.PostScripts);
                    NormalizeUrls(f.FragmentOptions.PreScripts);
                    NormalizeUrls(f.FragmentOptions.Styles);
                });
            }
        }

        private void NormalizeUrls(string[] urlArray)
        {
            var fragmetSourceUri = new Uri(_fragmentSourceUrl);
            for (int i = 0; i < urlArray.Length; i++)
            {
                if (urlArray[i].StartsWith("http") == false)
                {
                    urlArray[i] = new UriBuilder(fragmetSourceUri.Host) { Path = urlArray[i] }.Uri.ToString();
                }
            }
        }
    }
}
