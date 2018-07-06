using System.Threading.Tasks;

namespace AspNetCore.Mvc.Fragments.Renderer
{
    public interface IViewRenderer
    {
        Task<string> RenderAsync(string viewName, object model);
    }
}
