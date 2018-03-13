using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace AspNetCore.Mvc.Fragments.Renderer
{
    public class FragmentRenderer : IFragmentRenderer
    {
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;

        public FragmentRenderer(ICompositeViewEngine viewEngine, ITempDataProvider tempDataProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
        }

        public async Task RenderAsync(FragmentContext context)
        {
            var fragmentType = context.Fragment.GetType();
            if (fragmentType.GetMethod(Constants.ProcessAsyncMethodName).Invoke(context.Fragment, new[] { context.Model }) is Task<FragmentViewResult> fragmentProcessTask)
            {
                var preScripts = context.Fragment.Resources.PreScripts.Select(s => $"<script async src='{s}'></script>");
                await context.HttpContext.Response.WriteAsync(string.Concat(preScripts));
                await context.HttpContext.Response.Body.FlushAsync();

                var htmlString = await GetRazorViewHtmlAsync(await fragmentProcessTask, context);

                var scriptElementId = Guid.NewGuid().ToString();
                var postScripts = context.Fragment.Resources.PostScripts.Select(s => $"<script async src='{s}'></script>");
                htmlString = $"<!-- FRAGMENT_START:{fragmentType.Name} -->{htmlString}<!-- FRAGMENT_END:{fragmentType.Name} -->";
                await context.HttpContext.Response.WriteAsync($"<script id='{scriptElementId}'>document.getElementById('{context.PlaceHolderId}').outerHTML = '{htmlString}';var scriptElement = document.getElementById('{scriptElementId}'); scriptElement.parentNode.removeChild(scriptElement);</script>{string.Concat(postScripts)}");
                await context.HttpContext.Response.Body.FlushAsync();
            }
        }

        private async Task<string> GetRazorViewHtmlAsync(FragmentViewResult fragmentViewResult, FragmentContext fragmentContext)
        {
            var actionContext = new ActionContext(fragmentContext.HttpContext, new RouteData(), new ActionDescriptor());
            var viewEngineResult = _viewEngine.FindView(actionContext, fragmentViewResult.ViewName, false);

            if (!viewEngineResult.Success)
                throw new InvalidOperationException(string.Format("Couldn't find view '{0}'. Searched paths : '{1}'", fragmentViewResult.ViewName, string.Join(",", viewEngineResult.SearchedLocations)));

            var view = viewEngineResult.View;

            using (var output = new StringWriter())
            {
                var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = fragmentViewResult.Model };
                var tempData = new TempDataDictionary(actionContext.HttpContext, _tempDataProvider);
                var viewContext = new ViewContext(actionContext, view, viewData, tempData, output, new HtmlHelperOptions());

                await view.RenderAsync(viewContext);

                return output.ToString();
            }
        }
    }
}