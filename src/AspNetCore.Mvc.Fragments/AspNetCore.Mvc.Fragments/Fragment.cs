using System.Threading.Tasks;

namespace AspNetCore.Mvc.Fragments
{
    public abstract class Fragment
    {
        public virtual async Task<string> GetPlaceHolderHtmlAsync()
        {
            return await Task.FromResult(string.Empty);
        }

        public FragmentViewResult View(string viewName, object model)
        {
            return new FragmentViewResult(viewName, model);
        }
    }

    public class FragmentViewResult
    {
        public string ViewName { get; }
        public object Model { get; }

        public FragmentViewResult(string viewName, object model)
        {
            ViewName = viewName;
            Model = model;
        }
    }
}
