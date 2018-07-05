using AspNetCore.Mvc.Fragments.Options;

namespace AspNetCore.Mvc.Fragments
{
    public class FragmentInfo
    {
        public string Name { get; set; }
        public IFragmentOptions FragmentOptions { get; set; }
    }
}