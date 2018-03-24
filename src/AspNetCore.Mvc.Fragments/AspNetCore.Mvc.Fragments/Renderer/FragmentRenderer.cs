using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
            if (fragmentType.GetMethod(Constants.ProcessAsyncMethodName).Invoke(context.Fragment, context.Model == null ? new object[0] : new[] { context.Model }) is Task<FragmentViewResult> fragmentProcessTask)
            {
                var fragmentViewResult = (await fragmentProcessTask);
                var htmlString = await _viewRenderer.RenderAsync(fragmentViewResult.ViewName, fragmentViewResult.Model);

                var scriptElementId = Guid.NewGuid().ToString();
                var postScripts = context.FragmentOptions.PostScripts.Select(s => $"<script async src='{s}'></script>");
                htmlString = $"<!-- FRAGMENT_START:{fragmentType.Name} -->{htmlString.Replace("\r\n", string.Empty).Replace("'", "\\'")}<!-- FRAGMENT_END:{fragmentType.Name} -->";
                await context.OutputStream.WriteAsync($"<script id='{scriptElementId}'>document.getElementById('{context.PlaceHolderId}').outerHTML = '{htmlString}';var scriptElement = document.getElementById('{scriptElementId}'); scriptElement.parentNode.removeChild(scriptElement);</script>{string.Concat(postScripts)}");
                await context.OutputStream.FlushAsync();
            }
        }
    }
}