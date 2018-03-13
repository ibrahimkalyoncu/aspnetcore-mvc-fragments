using System;
using AspNetCore.Mvc.Fragments.Filters;
using AspNetCore.Mvc.Fragments.Renderer;
using AspNetCore.Mvc.Fragments.Resolver;
using Microsoft.Extensions.DependencyInjection;

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

            mvcBuilder.Services.AddScoped(typeof(IFragmentRenderer), fragmentMvcBuilderOptions.FragmentRendererType);
            mvcBuilder.Services.AddScoped(typeof(IFragmentResolver), fragmentMvcBuilderOptions.FragmentResolverType);
            return mvcBuilder.AddMvcOptions(option => option.Filters.Add(new FragmentResultFilter()));
        }
    }
}
