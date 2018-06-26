using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCore.Mvc.Fragments.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString FragmentStyles(this IHtmlHelper helper)
        {
            return new HtmlString(Constants.FragmentStylePlaceHolder);
        }

        public static HtmlString FragmentScripts(this IHtmlHelper helper)
        {
            return new HtmlString(Constants.FragmentScriptPlaceHolder);
        }
    }
}
