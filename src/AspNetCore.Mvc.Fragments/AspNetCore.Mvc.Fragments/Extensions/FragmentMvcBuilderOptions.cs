using System;
using AspNetCore.Mvc.Fragments.Context;
using AspNetCore.Mvc.Fragments.Renderer;
using AspNetCore.Mvc.Fragments.Resolver;

namespace AspNetCore.Mvc.Fragments.Extensions
{
    public class FragmentMvcBuilderOptions
    {
        internal Type FragmentResolverType { get; private set; }
        internal Type FragmentRendererType { get; private set; }
        internal Type ViewRendererType { get; private set; }
        internal Type FragmentContextProviderType { get; private set; }

        public FragmentMvcBuilderOptions()
        {
            FragmentResolverType = typeof(FragmentResolver);
            FragmentRendererType = typeof(FragmentRenderer);
            ViewRendererType = typeof(ViewRenderer);
            FragmentContextProviderType = typeof(FragmentContextProvider);
        }

        public void SetFragmentResolver<T>() where T : IFragmentResolver
        {
            FragmentResolverType = typeof(T);
        }

        public void SetFragmentRenderer<T>() where T : IFragmentRenderer
        {
            FragmentRendererType = typeof(T);
        }

        public void SetViewRenderer<T>() where T : IViewRenderer
        {
            ViewRendererType = typeof(T);
        }

        public void SetFragmentContextProvider<T>() where T : IFragmentContextProvider
        {
            FragmentContextProviderType = typeof(T);
        }
    }
}