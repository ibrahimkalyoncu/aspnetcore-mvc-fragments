using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCore.Mvc.Fragments.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent FragmentStyles(this IHtmlHelper html)
        {
            return new HtmlString(Constants.FragmentStylesPlaceHolder);
        }
    }
}
