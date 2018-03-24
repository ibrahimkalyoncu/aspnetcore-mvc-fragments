using System;
using System.IO;
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
    public interface IViewRenderer
    {
        Task<string> RenderAsync(string viewName, object model);
    }

    internal class ViewRenderer : IViewRenderer
    {
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _provider;

        public ViewRenderer(ICompositeViewEngine viewEngine, ITempDataProvider tempDataProvider, IServiceProvider provider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _provider = provider;
        }

        public async Task<string> RenderAsync(string viewName, object model)
        {
            var actionContext = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
            actionContext.HttpContext.RequestServices = _provider;
            var viewEngineResult = _viewEngine.FindView(actionContext, viewName, false);

            if (!viewEngineResult.Success)
                throw new InvalidOperationException(string.Format("Couldn't find view '{0}'. Searched paths : '{1}'", viewName, string.Join(",", viewEngineResult.SearchedLocations)));

            var view = viewEngineResult.View;

            using (var output = new StringWriter())
            {
                var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary()) { Model = model };
                var tempData = new TempDataDictionary(actionContext.HttpContext, _tempDataProvider);
                var viewContext = new ViewContext(actionContext, view, viewData, tempData, output, new HtmlHelperOptions());

                await view.RenderAsync(viewContext);

                return output.ToString();
            }
        }
    }
}
