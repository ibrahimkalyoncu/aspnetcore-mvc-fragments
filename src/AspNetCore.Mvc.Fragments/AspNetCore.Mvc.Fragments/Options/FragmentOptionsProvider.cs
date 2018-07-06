using System.Reflection;
using AspNetCore.Mvc.Fragments.Attributes;
using AspNetCore.Mvc.Fragments.Remote;

namespace AspNetCore.Mvc.Fragments.Options
{
    public class FragmentOptionsProvider : IFragmentOptionsProvider
    {
        public IFragmentOptions GetFragmentOptions(Fragment fragment)
        {
            if (fragment is RemoteFragment remoteFragment)
            {
                return remoteFragment.Options;
            }
            return fragment.GetType().GetCustomAttribute<FragmentOptionsAttribute>();
        }
    }
}