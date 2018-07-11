using System;
using System.Threading.Tasks;
using System.Linq;
using AspNetCore.Mvc.Fragments.Context;
using AspNetCore.Mvc.Fragments.Extensions;
using AspNetCore.Mvc.Fragments.Log;
using AspNetCore.Mvc.Fragments.Options;
using AspNetCore.Mvc.Fragments.Renderer;
using AspNetCore.Mvc.Fragments.Resolver;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Newtonsoft.Json;

namespace AspNetCore.Mvc.Fragments.TagHelpers
{
    [HtmlTargetElement("fragment")]
    public class FragmentTagHelper : TagHelper
    {
        private readonly IFragmentResolver _fragmentResolver;
        private readonly IViewRenderer _viewRenderer;
        private readonly IFragmentContextProvider _fragmentContextProvider;
        private readonly IFragmentOptionsProvider _fragmentOptionsProvider;
        private readonly IFragmentLogger _fragmentLogger;

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public string Name { get; set; }
        public object Model { get; set; }

        public FragmentTagHelper(IFragmentResolver fragmentResolver, 
            IViewRenderer viewRenderer, 
            IFragmentContextProvider fragmentContextProvider, 
            IFragmentOptionsProvider fragmentOptionsProvider,
            IFragmentLogger fragmentLogger)
        {
            _fragmentResolver = fragmentResolver;
            _viewRenderer = viewRenderer;
            _fragmentContextProvider = fragmentContextProvider;
            _fragmentOptionsProvider = fragmentOptionsProvider;
            _fragmentLogger = fragmentLogger;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            Fragment fragment = await _fragmentResolver.ResolveAsync(Name);

            var fragmentOptions = _fragmentOptionsProvider.GetFragmentOptions(fragment);

            var fragmentContext = new FragmentContext
            {
                Fragment = fragment,
                OutputStream = ViewContext.HttpContext.Response.Body,
                Model = Model,
                FragmentOptions = fragmentOptions
            };

            _fragmentContextProvider.AddContext(fragmentContext);

            _fragmentLogger.Info($"Executing fragment {Name}. Model : {JsonConvert.SerializeObject(fragmentContext.Model)}");

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
