using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using AspNetCore.Mvc.Fragments.Context;
using AspNetCore.Mvc.Fragments.Renderer;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.Mvc.Fragments.Filters
{
    public class FragmentResultFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            var fragmentContextProvider = context.HttpContext.RequestServices.GetService<IFragmentContextProvider>();
            var fragmentContexts = fragmentContextProvider?.GetContexts()?.ToList();
            if (fragmentContexts != null && fragmentContexts.Any())
            {
                IFragmentRenderer renderer = context.HttpContext.RequestServices.GetService<IFragmentRenderer>();
                Task.WaitAll(fragmentContexts.Select(renderer.RenderAsync).ToArray());
            }
        }
    }
}
