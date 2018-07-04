using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace AspNetCore.Mvc.Fragments.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IRouteBuilder MapFragmentRoute(this IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Fragment", "fragment/{name}", new { controller = "Fragment", action = "Index" });
            routeBuilder.MapRoute("FragmentContent", "fragment/{name}/content", new { controller = "Fragment", action = "Content" });
            routeBuilder.MapRoute("FragmentPlaceholder", "fragment/{name}/placeholder", new { controller = "Fragment", action = "Placeholder" });
            return routeBuilder;
        }
    }
}
