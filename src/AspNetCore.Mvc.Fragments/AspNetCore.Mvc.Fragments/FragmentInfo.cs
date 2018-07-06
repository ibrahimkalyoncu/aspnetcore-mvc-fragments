using AspNetCore.Mvc.Fragments.Options;

namespace AspNetCore.Mvc.Fragments
{
    public class FragmentInfo
    {
        public string Name { get; set; }
        public string Source { get; internal set; }
        public bool IsRemote { get; internal set; }
        public IFragmentOptions FragmentOptions { get; set; }
    }
}