using System;
using System.Threading.Tasks;

namespace AspNetCore.Mvc.Fragments.Demo.FragmentSource2.Services
{
    public class ContentService : IContentService
    {
        public async Task<string> GetMainContentAsync()
        {
            var randomValue = new Random().Next(750, 1000);
            await Task.Delay(randomValue);
            return "Use this document as a way to quickly start any new project.<br> All you get is this text and a mostly barebones HTML document.";
        }
    }
}