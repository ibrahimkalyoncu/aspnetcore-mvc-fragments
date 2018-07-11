using System;
using System.Collections.Generic;
using AspNetCore.Mvc.Fragments.Extensions;
using AspNetCore.Mvc.Fragments.Filters;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.Mvc.Fragments.Options
{
    public class FragmentApplicationBuilderOptions
    {
        internal List<Func<HttpContext,FragmentResponseFilter>> ResponseFilters { get; set; }
        internal bool IsGzipEnabled { get; set; }

        public FragmentApplicationBuilderOptions()
        {
            ResponseFilters = new List<Func<HttpContext, FragmentResponseFilter>>();
        }

        public FragmentApplicationBuilderOptions AddResponseFilter(Func<HttpContext, FragmentResponseFilter> responseFilterCreator)
        {
            ResponseFilters.Add(responseFilterCreator);
            return this;
        }

        public FragmentApplicationBuilderOptions EnableGzip()
        {
            IsGzipEnabled = true;
            return this;
        }
    }
}