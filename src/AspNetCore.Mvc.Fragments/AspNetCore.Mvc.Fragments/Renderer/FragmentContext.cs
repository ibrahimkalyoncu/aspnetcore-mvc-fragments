using Microsoft.AspNetCore.Http;

namespace AspNetCore.Mvc.Fragments.Renderer
{
    public class FragmentContext
    {
        public Fragment Fragment { get; set; }
        public HttpContext HttpContext { get; set; }
        public object Model { get; set; }
        public string PlaceHolderId { get; set; }
    }
}