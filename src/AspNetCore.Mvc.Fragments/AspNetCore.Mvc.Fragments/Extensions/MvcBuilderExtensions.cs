using System;
using System.Reflection;
using AspNetCore.Mvc.Fragments.Context;
using AspNetCore.Mvc.Fragments.Datasource;
using AspNetCore.Mvc.Fragments.Filters;
using AspNetCore.Mvc.Fragments.Options;
using AspNetCore.Mvc.Fragments.Registry;
using AspNetCore.Mvc.Fragments.Renderer;
using AspNetCore.Mvc.Fragments.Resolver;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace AspNetCore.Mvc.Fragments.Extensions
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddFragments(this IMvcBuilder mvcBuilder)
        {
            return AddFragments(mvcBuilder, null);
        }

        public static IMvcBuilder AddFragments(this IMvcBuilder mvcBuilder, Action<FragmentMvcBuilderOptions> optionsBuilder)
        {
            FragmentMvcBuilderOptions fragmentMvcBuilderOptions = new FragmentMvcBuilderOptions();

            optionsBuilder?.Invoke(fragmentMvcBuilderOptions);

            mvcBuilder.Services.Configure<RazorViewEngineOptions>(options => options.FileProviders.Add(new EmbeddedFileProvider(typeof(Fragment).Assembly)));
            mvcBuilder.Services.AddScoped(typeof(IViewRenderer), fragmentMvcBuilderOptions.ViewRendererType);
            mvcBuilder.Services.AddScoped(typeof(IFragmentRenderer), fragmentMvcBuilderOptions.FragmentRendererType);
            mvcBuilder.Services.AddScoped(typeof(IFragmentResolver), fragmentMvcBuilderOptions.FragmentResolverType);
            mvcBuilder.Services.AddScoped(typeof(IFragmentContextProvider), fragmentMvcBuilderOptions.FragmentContextProviderType);
            mvcBuilder.Services.AddSingleton(typeof(IFragmentOptionsProvider), fragmentMvcBuilderOptions.FragmentOptionsProviderType);

            var fragmentRegistry = new FragmentRegistry();
            fragmentRegistry.AddDatasource(new FragmentAssamblyDatasource(Assembly.GetEntryAssembly()));
            mvcBuilder.Services.AddSingleton(typeof(IFragmentRegistry),fragmentRegistry);

            return mvcBuilder.AddMvcOptions(option => option.Filters.Add(new FragmentResultFilter()));
        }
    }
}
