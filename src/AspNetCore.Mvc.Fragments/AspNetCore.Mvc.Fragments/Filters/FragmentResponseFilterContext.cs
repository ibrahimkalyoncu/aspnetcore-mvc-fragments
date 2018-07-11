using System.IO;

namespace AspNetCore.Mvc.Fragments.Filters
{
    public class FragmentResponseFilterContext
    {
        public Stream BodyStream { get; set; }
        public int FlushIndex { get; set; }
        public string ResponseHtml { get; set; }
    }
}