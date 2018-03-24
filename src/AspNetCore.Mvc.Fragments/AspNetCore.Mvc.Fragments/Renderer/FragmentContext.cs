using System.IO;
using AspNetCore.Mvc.Fragments.Attributes;
using Microsoft.AspNetCore.Http;

namespace AspNetCore.Mvc.Fragments.Renderer
{
    public class FragmentContext
    {
        public Fragment Fragment { get; set; }
        public Stream OutputStream { get; set; }
        public object Model { get; set; }
        public string PlaceHolderId { get; set; }
        public FragmentOptionsAttribute FragmentOptions { get; set; }
    }
}