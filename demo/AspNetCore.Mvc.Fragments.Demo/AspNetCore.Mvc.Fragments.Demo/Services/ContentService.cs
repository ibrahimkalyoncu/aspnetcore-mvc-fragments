using System.Threading.Tasks;

namespace AspNetCore.Mvc.Fragments.Demo.Services
{
    public class ContentService : IContentService
    {
        public async Task<string> GetMainContentAsync()
        {
            await Task.Delay(3000);
            return "Use this document as a way to quickly start any new project.<br> All you get is this text and a mostly barebones HTML document.";
        }
    }
}