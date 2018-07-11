using System;
using System.Linq;
using AspNetCore.Mvc.Fragments.Context;
using AspNetCore.Mvc.Fragments.Filters;
using AspNetCore.Mvc.Fragments.Options;
using AspNetCore.Mvc.Fragments.Streams;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.Mvc.Fragments.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IRouteBuilder MapFragmentRoute(this IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("FragmentList", "fragment", new { controller = "Fragment", action = "List" });
            routeBuilder.MapRoute("FragmentPreview", "fragment/{name}", new { controller = "Fragment", action = "Preview" });
            routeBuilder.MapRoute("FragmentContent", "fragment/{name}/content/{mode?}", new { controller = "Fragment", action = "Content" });
            routeBuilder.MapRoute("FragmentPlaceholder", "fragment/{name}/placeholder/{mode?}", new { controller = "Fragment", action = "Placeholder" });
            return routeBuilder;
        }

        public static IApplicationBuilder UseFragments(this IApplicationBuilder applicationBuilder)
        {
            return UseFragments(applicationBuilder, null);
        }

        public static IApplicationBuilder UseFragments(this IApplicationBuilder applicationBuilder, Action<FragmentApplicationBuilderOptions> optionsBuilder)
        {
            var options = new FragmentApplicationBuilderOptions();
            options.AddResponseFilter(context => new DefaultFragmentResponseFilter(context.RequestServices.GetService<IFragmentContextProvider>()));
            optionsBuilder?.Invoke(options);

            applicationBuilder.Use(async (context, next) =>
            {
                context.Response.Body = new FragmentResponseStream(context, options.ResponseFilters.Select(creator => creator(context)).ToList(), options.IsGzipEnabled);
                await next();
            });

            return applicationBuilder;
        }
    }
}
