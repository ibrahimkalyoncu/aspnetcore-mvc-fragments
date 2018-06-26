using System.Collections.Generic;

namespace AspNetCore.Mvc.Fragments.Context
{
    public interface IFragmentContextProvider
    {
        IReadOnlyCollection<FragmentContext> GetContexts();
        void AddContext(FragmentContext context);
    }
}
