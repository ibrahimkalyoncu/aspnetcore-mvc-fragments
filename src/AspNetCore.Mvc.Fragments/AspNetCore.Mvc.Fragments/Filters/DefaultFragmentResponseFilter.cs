using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Mvc.Fragments.Context;
using AspNetCore.Mvc.Fragments.Extensions;

namespace AspNetCore.Mvc.Fragments.Filters
{
    public class DefaultFragmentResponseFilter : FragmentResponseFilter
    {
        private readonly IFragmentContextProvider _contextProvider;

        public DefaultFragmentResponseFilter(IFragmentContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public override void OnRendering(FragmentResponseFilterContext context)
        {
            if (context.FlushIndex == 0)
            {
                var styles = _contextProvider.GetContexts().SelectMany(fragmentContext =>
                    fragmentContext.FragmentOptions.Styles.Select(style => $"<link  rel='stylesheet' href='{style}'/>")).Distinct();

                var stylesHtmlString = string.Join(Environment.NewLine, styles);

                context.ResponseHtml = context.ResponseHtml.Replace(Constants.FragmentStylesPlaceHolder, stylesHtmlString);
            }
        }
    }
}