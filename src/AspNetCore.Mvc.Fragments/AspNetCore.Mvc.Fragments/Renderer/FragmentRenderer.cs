using System;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Mvc.Fragments.Context;
using AspNetCore.Mvc.Fragments.Extensions;

namespace AspNetCore.Mvc.Fragments.Renderer
{
    internal class FragmentRenderer : IFragmentRenderer
    {
        private readonly IViewRenderer _viewRenderer;

        public FragmentRenderer(IViewRenderer viewRenderer)
        {
            _viewRenderer = viewRenderer;
        }

        public async Task RenderAsync(FragmentContext context)
        {
            var fragmentType = context.Fragment.GetType();
            var htmlString = await ExecuteAsync(context);
            var scriptElementId = Guid.NewGuid().ToString();
            var postScripts = context.FragmentOptions.PostScripts.Select(s => $"<script async src='{s}'></script>");
            htmlString = $"<!-- FRAGMENT_START:{fragmentType.Name} -->{htmlString.Replace("\r\n", string.Empty).Replace("'", "\\'")}<!-- FRAGMENT_END:{fragmentType.Name} -->";
            await context.OutputStream.WriteAsync($"<script id='{scriptElementId}'>document.getElementById('{context.PlaceHolderId}').outerHTML = '{htmlString}';var scriptElement = document.getElementById('{scriptElementId}'); scriptElement.parentNode.removeChild(scriptElement);</script>{string.Concat(postScripts)}");
            await context.OutputStream.FlushAsync();
        }

        public async Task<string> ExecuteAsync(FragmentContext context)
        {
            var fragmentType = context.Fragment.GetType();
            var processMethodInfo = fragmentType.GetMethod(Constants.ProcessAsyncMethodName);
            var parameterInfos = processMethodInfo.GetParameters();

            if (processMethodInfo.Invoke(context.Fragment, GetModel(parameterInfos?.Length > 0 ? parameterInfos[0].ParameterType : null ,context)) is Task<FragmentViewResult> fragmentProcessTask)
            {
                var fragmentViewResult = (await fragmentProcessTask);
                var htmlString = await _viewRenderer.RenderAsync(fragmentViewResult.ViewName, fragmentViewResult.Model);
                return htmlString;
            }

            return string.Empty;
        }

        private static object[] GetModel(Type modelType, FragmentContext context)
        {
            if (modelType == null || context.Model == null)
            {
                return new object[0];
            }

            if (context.Model.GetType().FullName != modelType.FullName)
            {
                context.Model = Newtonsoft.Json.JsonConvert.DeserializeObject(Newtonsoft.Json.JsonConvert.SerializeObject(context.Model), modelType);
            }

            return context.Model == null ? new object[0] : new[] { context.Model };
        }
    }
}