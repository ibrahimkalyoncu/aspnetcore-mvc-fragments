using System.Threading.Tasks;

namespace AspNetCore.Mvc.Fragments.Demo.FragmentSource2.Services
{
    public interface IContentService
    {
        Task<string> GetMainContentAsync();
    }
}