using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCore.Mvc.Fragments.Renderer;
using AspNetCore.Mvc.Fragments.Resolver;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspNetCore.Mvc.Fragments.TagHelpers
{
    [HtmlTargetElement("fragment")]
    public class FragmentTagHelper : TagHelper
    {
        private readonly IFragmentResolver _fragmentResolver;

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public string Name { get; set; }
        public object Model { get; set; }

        public FragmentTagHelper(IFragmentResolver fragmentResolver)
        {
            _fragmentResolver = fragmentResolver;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            Fragment fragment = _fragmentResolver.Resolve(Name);
            output.TagName = "div";

            string placeHolderHtml = await fragment.GetPlaceHolderHtmlAsync();

            if (string.IsNullOrEmpty(placeHolderHtml) == false)
                output.Content.SetContent(placeHolderHtml);

            var placeHolderId = Guid.NewGuid().ToString();
            output.Attributes.SetAttribute(Constants.HtmlIdAttribute, placeHolderId);
            output.TagMode = TagMode.StartTagAndEndTag;

            ((ViewContext.HttpContext.Items[Constants.HttpContexItemsFragmentCollectionKey] = ViewContext.HttpContext.Items[Constants.HttpContexItemsFragmentCollectionKey]
                ?? new List<FragmentContext>()) as List<FragmentContext>)?.Add(new FragmentContext
                {
                    Fragment = fragment,
                    HttpContext = ViewContext.HttpContext,
                    Model = Model,
                    PlaceHolderId = placeHolderId
                });
        }
    }
}
