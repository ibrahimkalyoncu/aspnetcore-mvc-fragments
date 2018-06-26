using System.Threading.Tasks;
using AspNetCore.Mvc.Fragments.Context;

namespace AspNetCore.Mvc.Fragments.Renderer
{
    public interface IFragmentRenderer
    {
        Task RenderAsync(FragmentContext context);
    }
}