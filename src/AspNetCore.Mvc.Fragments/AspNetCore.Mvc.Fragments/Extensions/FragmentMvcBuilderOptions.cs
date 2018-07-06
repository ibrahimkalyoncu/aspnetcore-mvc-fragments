using System;
using System.Collections.Generic;
using AspNetCore.Mvc.Fragments.Context;
using AspNetCore.Mvc.Fragments.Datasource;
using AspNetCore.Mvc.Fragments.Options;
using AspNetCore.Mvc.Fragments.Renderer;
using AspNetCore.Mvc.Fragments.Resolver;

namespace AspNetCore.Mvc.Fragments.Extensions
{
    public class FragmentMvcBuilderOptions
    {
        internal List<IFragmentDatasource> Datasources;
        internal Type FragmentResolverType { get; private set; }
        internal Type FragmentRendererType { get; private set; }
        internal Type ViewRendererType { get; private set; }
        internal Type FragmentContextProviderType { get; private set; }
        internal Type FragmentOptionsProviderType { get; private set; }

        public FragmentMvcBuilderOptions()
        {
            Datasources = new List<IFragmentDatasource>();

            FragmentResolverType = typeof(FragmentResolver);
            FragmentRendererType = typeof(FragmentRenderer);
            ViewRendererType = typeof(ViewRenderer);
            FragmentContextProviderType = typeof(FragmentContextProvider);
            FragmentOptionsProviderType = typeof(FragmentOptionsProvider);
        }

        public FragmentMvcBuilderOptions SetFragmentResolver<T>() where T : IFragmentResolver
        {
            FragmentResolverType = typeof(T);
            return this;
        }

        public FragmentMvcBuilderOptions SetFragmentRenderer<T>() where T : IFragmentRenderer
        {
            FragmentRendererType = typeof(T);
            return this;
        }

        public FragmentMvcBuilderOptions SetViewRenderer<T>() where T : IViewRenderer
        {
            ViewRendererType = typeof(T);
            return this;
        }

        public FragmentMvcBuilderOptions SetFragmentContextProvider<T>() where T : IFragmentContextProvider
        {
            FragmentContextProviderType = typeof(T);
            return this;
        }

        public FragmentMvcBuilderOptions SetFragmentOptionsProvider<T>() where T : IFragmentOptionsProvider
        {
            FragmentOptionsProviderType = typeof(T);
            return this;
        }

        public FragmentMvcBuilderOptions AddDatasource(IFragmentDatasource datasource)
        {
            Datasources.Add(datasource);
            return this;
        }
    }
}