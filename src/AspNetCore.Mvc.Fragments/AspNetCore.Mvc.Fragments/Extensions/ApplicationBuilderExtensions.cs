using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace AspNetCore.Mvc.Fragments.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IRouteBuilder MapFragmentRoute(this IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("FragmentList", "fragment", new { controller = "Fragment", action = "List" });
            routeBuilder.MapRoute("FragmentPreview", "fragment/{name}", new { controller = "Fragment", action = "Preview" });
            routeBuilder.MapRoute("FragmentContent", "fragment/{name}/content", new { controller = "Fragment", action = "Content" });
            routeBuilder.MapRoute("FragmentPlaceholder", "fragment/{name}/placeholder", new { controller = "Fragment", action = "Placeholder" });
            return routeBuilder;
        }
    }
}
