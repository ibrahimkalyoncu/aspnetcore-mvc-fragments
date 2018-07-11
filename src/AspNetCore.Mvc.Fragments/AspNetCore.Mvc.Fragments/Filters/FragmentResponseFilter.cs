namespace AspNetCore.Mvc.Fragments.Filters
{
    public abstract class FragmentResponseFilter
    {
        public virtual void OnRendering(FragmentResponseFilterContext context)
        {
        }

        public virtual void OnRendered(FragmentResponseFilterContext context)
        {
        }

        public virtual void OnComplete()
        {
        }
    }
}