using System.Threading.Tasks;
using AspNetCore.Mvc.Fragments.Context;

namespace AspNetCore.Mvc.Fragments.Renderer
{
    public interface IFragmentRenderer
    {
        /// <summary>
        /// Renders given fragment context into executing response stream as html
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task RenderAsync(FragmentContext context);

        /// <summary>
        /// Executes given fragment context and returns a html result
        /// </summary>
        /// <param name="context"></param>
        /// <returns>Html result</returns>
        Task<string> ExecuteAsync(FragmentContext context);
    }
}