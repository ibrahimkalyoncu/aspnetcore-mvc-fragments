using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AspNetCore.Mvc.Fragments.Renderer;
using Microsoft.AspNetCore.Mvc.Filters;
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
            context.HttpContext.Response.Body.FlushAsync();
            
            if (context.HttpContext.Items[Constants.HttpContexItemsFragmentCollectionKey] is List<FragmentContext> contexts && contexts.Any())
            {
                IFragmentRenderer renderer = context.HttpContext.RequestServices.GetService<IFragmentRenderer>();
                Task.WaitAll(contexts.Select(renderer.RenderAsync).ToArray());
            }
        }
    }
}
