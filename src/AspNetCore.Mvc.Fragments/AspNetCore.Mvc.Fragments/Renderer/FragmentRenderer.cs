using System;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Mvc.Fragments.Context;
using AspNetCore.Mvc.Fragments.Extensions;
using AspNetCore.Mvc.Fragments.Remote;

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
            var contentWrapperElementId = Guid.NewGuid().ToString();
            var postScripts = context.FragmentOptions.PostScripts.Select(s => $"<script async src='{s}'></script>");
            htmlString = $"<!-- FRAGMENT_START:{fragmentType.Name} --><div style='position:absolute; top:-9999px;bottom:-9999px;left:-9999px' id='{contentWrapperElementId}'>{htmlString.Replace("\r\n", string.Empty).Replace("'", "\\'")}</div><!-- FRAGMENT_END:{fragmentType.Name} -->";

            string removeScriptElementScript = $";var scriptElement = document.getElementById('{scriptElementId}'); scriptElement.parentNode.removeChild(scriptElement);";
            string removeContentWrapperElementScript = $";var contentWrapperElement = document.getElementById('{contentWrapperElementId}'); contentWrapperElement.parentNode.removeChild(contentWrapperElement);";
            string replaceContentScript = $";document.getElementById('{context.PlaceHolderId}').outerHTML = document.getElementById('{contentWrapperElementId}').innerHTML;";

            await context.OutputStream.WriteAsync($"{htmlString}<script id='{scriptElementId}'>{replaceContentScript}{removeContentWrapperElementScript}{removeScriptElementScript}</script>{string.Concat(postScripts)}");
        }

        public async Task<string> ExecuteAsync(FragmentContext context)
        {
            var fragmentType = context.Fragment.GetType();
            var processMethodInfo = fragmentType.GetMethod(Constants.ProcessAsyncMethodName);
            var parameterInfos = processMethodInfo.GetParameters();

            if (context.Fragment is RemoteFragment)
            {
                if (processMethodInfo.Invoke(context.Fragment, new object[1] { context.Model ?? new object() }) is Task<string> html)
                {
                    return await html;
                }

                return string.Empty;
            }

            if (processMethodInfo.Invoke(context.Fragment, GetModel(parameterInfos?.Length > 0 ? parameterInfos[0].ParameterType : null, context)) is Task<FragmentViewResult> fragmentProcessTask)
            {
                var fragmentViewResult = await fragmentProcessTask;
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