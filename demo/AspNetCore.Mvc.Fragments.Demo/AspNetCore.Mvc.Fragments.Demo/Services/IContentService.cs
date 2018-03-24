using System.Threading.Tasks;

namespace AspNetCore.Mvc.Fragments.Demo.Services
{
    public interface IContentService
    {
        Task<string> GetMainContentAsync();
    }
}