using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Mvc.Fragments.Context;
using AspNetCore.Mvc.Fragments.Options;
using AspNetCore.Mvc.Fragments.Registry;
using AspNetCore.Mvc.Fragments.Renderer;
using AspNetCore.Mvc.Fragments.Resolver;
using AspNetCore.Mvc.Fragments.Views;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Mvc.Fragments.Controllers
{
    public class FragmentController : Controller
    {
        private readonly IFragmentResolver _fragmentResolver;
        private readonly IFragmentRenderer _fragmentRenderer;
        private readonly IFragmentOptionsProvider _fragmentOptionsProvider;
        private readonly IViewRenderer _viewRenderer;
        private readonly IFragmentRegistry _fragmentRegistry;

        public FragmentController(IFragmentResolver fragmentResolver,
            IFragmentRenderer fragmentRenderer,
            IFragmentOptionsProvider fragmentOptionsProvider,
            IViewRenderer viewRenderer,
            IFragmentRegistry fragmentRegistry)
        {
            _fragmentResolver = fragmentResolver;
            _fragmentRenderer = fragmentRenderer;
            _fragmentOptionsProvider = fragmentOptionsProvider;
            _viewRenderer = viewRenderer;
            _fragmentRegistry = fragmentRegistry;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            return Json(await _fragmentRegistry.GetAllAsync());
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Preview([FromRoute]string name, [FromQuery][FromBody]dynamic model)
        {
            if (Request.Method == "GET")
            {
                var expObject = new ExpandoObject();
                foreach (var keyValuePair in Request.Query)
                {
                    ((IDictionary<string, Object>)expObject).Add(keyValuePair.Key, keyValuePair.Value.FirstOrDefault());
                }
                model = expObject;
            }

            return await Task.FromResult(View(new FragmentViewModel { FragmentModel = model, FragmentName = name }));
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Content([FromRoute]string name, [FromRoute] string mode, [FromQuery][FromBody]dynamic model)
        {
            var fragment = await _fragmentResolver.ResolveAsync(name);
            var fragmentOptions = _fragmentOptionsProvider.GetFragmentOptions(fragment);
            var fragmentType = fragment.GetType();

            var methodInfo = fragmentType.GetMethod(Constants.ProcessAsyncMethodName);
            var modelType = methodInfo.GetParameters().FirstOrDefault();

            var html = await _fragmentRenderer.ExecuteAsync(new FragmentContext
            {
                Fragment = fragment,
                FragmentOptions = fragmentOptions,
                Model = model ?? (modelType == null ? null : Activator.CreateInstance(modelType.ParameterType)),
                OutputStream = HttpContext.Response.Body,
                PlaceHolderId = Guid.Empty.ToString()
            });

            if (mode == "preview")
            {
                html = AddAssetsToHtml(html, fragmentOptions);
            }

            return base.Content(html, "text/html");
        }

        public async Task<IActionResult> Placeholder([FromRoute] string name, [FromRoute] string mode)
        {
            var fragment = await _fragmentResolver.ResolveAsync(name);
            var fragmentOptions = _fragmentOptionsProvider.GetFragmentOptions(fragment);

            var placeHolderHtml = string.IsNullOrEmpty(fragmentOptions.PlaceHolderViewName)
                ? await fragment.GetPlaceHolderHtmlAsync()
                : await _viewRenderer.RenderAsync(fragmentOptions.PlaceHolderViewName, null);

            if (mode == "preview")
            {
                placeHolderHtml = AddAssetsToHtml(placeHolderHtml, fragmentOptions);
            }

            return base.Content(placeHolderHtml, "text/html");
        }

        private static string AddAssetsToHtml(string html, IFragmentOptions fragmentOptions)
        {
            html = @"<style>
                body { 
                    background-size: 40px 40px;
                    background-image: linear-gradient(to right, #ddd 1px, transparent 1px), linear-gradient(to bottom, #ddd 1px, transparent 1px);
                }
            </style>" + html;

            foreach (var style in fragmentOptions.PreScripts)
            {
                html = $"<script type='text/javascript' src='{style}'></script>" + html;
            }

            foreach (var style in fragmentOptions.Styles)
            {
                html = $"<link rel='stylesheet' type='text/css' href='{style}'/>" + html;
            }

            foreach (var style in fragmentOptions.PostScripts)
            {
                html += $"<script type='text/javascript' src='{style}'></script>";
            }
            return html;
        }
    }
}
