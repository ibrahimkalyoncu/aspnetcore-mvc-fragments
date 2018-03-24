﻿using System;
using AspNetCore.Mvc.Fragments.Renderer;
using AspNetCore.Mvc.Fragments.Resolver;

namespace AspNetCore.Mvc.Fragments.Extensions
{
    public class FragmentMvcBuilderOptions
    {
        internal Type FragmentResolverType { get; private set; }
        internal Type FragmentRendererType { get; private set; }
        internal Type ViewRendererType { get; private set; }

        public FragmentMvcBuilderOptions()
        {
            FragmentResolverType = typeof(FragmentResolver);
            FragmentRendererType = typeof(FragmentRenderer);
            ViewRendererType = typeof(ViewRenderer);
        }

        public void SetFragmentResolver<T>() where T : IFragmentResolver
        {
            FragmentResolverType = typeof(T);
        }

        public void SetFragmentRenderer<T>() where T : IFragmentRenderer
        {
            FragmentRendererType = typeof(T);
        }

        public void SetViewRendererType<T>() where T : IViewRenderer
        {
            ViewRendererType = typeof(T);
        }
    }
}