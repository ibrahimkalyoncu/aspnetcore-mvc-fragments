using System.IO;
using AspNetCore.Mvc.Fragments.Attributes;
using AspNetCore.Mvc.Fragments.Options;

namespace AspNetCore.Mvc.Fragments.Context
{
    public class FragmentContext
    {
        public Fragment Fragment { get; set; }
        public Stream OutputStream { get; set; }
        public object Model { get; set; }
        public string PlaceHolderId { get; set; }
        public IFragmentOptions FragmentOptions { get; set; }
    }
}