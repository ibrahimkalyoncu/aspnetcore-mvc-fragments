using System.Threading.Tasks;

namespace AspNetCore.Mvc.Fragments.Resolver
{
    public interface IFragmentResolver
    {
        Task<Fragment> ResolveAsync(string name);
    }
}
