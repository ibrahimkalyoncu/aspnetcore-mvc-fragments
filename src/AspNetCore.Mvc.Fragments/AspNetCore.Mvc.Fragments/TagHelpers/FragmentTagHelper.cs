using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using AspNetCore.Mvc.Fragments.Attributes;
using AspNetCore.Mvc.Fragments.Extensions;
using AspNetCore.Mvc.Fragments.Renderer;
using AspNetCore.Mvc.Fragments.Resolver;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspNetCore.Mvc.Fragments.TagHelpers
{
    [HtmlTargetElement("fragment")]
    public class FragmentTagHelper : TagHelper
    {
        private readonly IFragmentResolver _fragmentResolver;
        private readonly IFragmentRenderer _fragmentRenderer;
        private readonly IViewRenderer _viewRenderer;

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public string Name { get; set; }
        public object Model { get; set; }

        public FragmentTagHelper(IFragmentResolver fragmentResolver, IFragmentRenderer fragmentRenderer, IViewRenderer viewRenderer)
        {
            _fragmentResolver = fragmentResolver;
            _fragmentRenderer = fragmentRenderer;
            _viewRenderer = viewRenderer;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            Fragment fragment = _fragmentResolver.Resolve(Name);

            var fragmentOptions = fragment.GetType().GetCustomAttribute(typeof(FragmentOptionsAttribute)) as FragmentOptionsAttribute;

            var fragmentContext = new FragmentContext
            {
                Fragment = fragment,
                OutputStream = ViewContext.HttpContext.Response.Body,
                Model = Model,
                FragmentOptions = fragmentOptions
            };

            await RenderPreAssestsAsync(fragmentContext);

            var placeHolderViewName = fragmentOptions?.PlaceHolderViewName;

            if (fragmentOptions?.IsSync == true || fragmentOptions?.IsSyncOnAjax == true && IsAjaxRequest(ViewContext.HttpContext.Request))
            {
                output.SuppressOutput();
                throw new NotImplementedException("Sync fragments are not implemented yet.");
            }
            else
            {
                fragmentContext.PlaceHolderId = Guid.NewGuid().ToString();

                var placeHolderHtml = string.IsNullOrEmpty(placeHolderViewName) ? await fragment.GetPlaceHolderHtmlAsync() : await _viewRenderer.RenderAsync(placeHolderViewName, null);
                if (string.IsNullOrEmpty(placeHolderHtml) == false)
                    output.Content.SetHtmlContent(placeHolderHtml);

                output.Attributes.SetAttribute(Constants.HtmlIdAttribute, fragmentContext.PlaceHolderId);
                output.TagMode = TagMode.StartTagAndEndTag;

                ((ViewContext.HttpContext.Items[Constants.HttpContexItemsFragmentCollectionKey] = ViewContext.HttpContext.Items[Constants.HttpContexItemsFragmentCollectionKey] ?? new List<FragmentContext>()) as List<FragmentContext>)?.Add(fragmentContext);
            }
        }

        private static async Task RenderPreAssestsAsync(FragmentContext fragmentContext)
        {
            var preScripts = fragmentContext.FragmentOptions.PreScripts.Select(s => $"<script async src='{s}'></script>");
            var styles = fragmentContext.FragmentOptions.Styles.Select(s => $"<link  rel='stylesheet' href='{s}'/>");
            await fragmentContext.OutputStream.WriteAsync(string.Concat(styles));
            await fragmentContext.OutputStream.WriteAsync(string.Concat(preScripts));
            await fragmentContext.OutputStream.FlushAsync();
        }

        private bool IsAjaxRequest(HttpRequest request)
        {
            if (request?.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";

            return false;
        }
    }
}
