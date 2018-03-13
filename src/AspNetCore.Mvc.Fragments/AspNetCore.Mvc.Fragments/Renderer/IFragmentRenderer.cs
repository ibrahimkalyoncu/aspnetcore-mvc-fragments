using System.Threading.Tasks;

namespace AspNetCore.Mvc.Fragments.Renderer
{
    public interface IFragmentRenderer
    {
        Task RenderAsync(FragmentContext context);
    }
}