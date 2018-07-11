using AspNetCore.Mvc.Fragments.Context;

namespace AspNetCore.Mvc.Fragments.Filters
{
    public class DefaultFragmentResponseFilter : FragmentResponseFilter
    {
        private readonly IFragmentContextProvider _contextProvider;

        public DefaultFragmentResponseFilter(IFragmentContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public override void OnRendering(FragmentResponseFilterContext context)
        {
            context.ResponseHtml = $"<!-- Start -->{context.ResponseHtml}<!-- End -->";
        }
    }
}