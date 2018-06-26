using System.Collections.Generic;
using AspNetCore.Mvc.Fragments.Renderer;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.Mvc.Fragments.Context
{
    public class FragmentContextProvider : IFragmentContextProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FragmentContextProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IReadOnlyCollection<FragmentContext> GetContexts()
        {
            return (_httpContextAccessor.HttpContext.Items[Constants.HttpContexItemsFragmentCollectionKey] =
                _httpContextAccessor.HttpContext.Items[Constants.HttpContexItemsFragmentCollectionKey] ??
                new List<FragmentContext>()) as IReadOnlyCollection<FragmentContext>;
        }

        public void AddContext(FragmentContext context)
        {
            (GetContexts() as List<FragmentContext>)?.Add(context);
        }
    }
}