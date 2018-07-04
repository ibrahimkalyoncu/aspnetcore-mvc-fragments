using System.Reflection;
using AspNetCore.Mvc.Fragments.Attributes;

namespace AspNetCore.Mvc.Fragments.Options
{
    public class FragmentOptionsProvider : IFragmentOptionsProvider
    {
        public IFragmentOptions GetFragmentOptions(Fragment fragment)
        {
            return fragment.GetType().GetCustomAttribute<FragmentOptionsAttribute>();
        }
    }
}