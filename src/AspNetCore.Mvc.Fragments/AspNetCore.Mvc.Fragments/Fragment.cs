using System.Threading.Tasks;

namespace AspNetCore.Mvc.Fragments
{
    public abstract class Fragment
    {
        //Attribute yaparsan serialize edip dışarı açabilirsin
        //Nesne new'lemene gerek kalmaz
        private FragmentResources _resources;
        public FragmentResources Resources => _resources ?? (_resources = new FragmentResources());

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
