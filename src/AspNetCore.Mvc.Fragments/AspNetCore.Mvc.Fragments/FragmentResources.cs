using System.Collections.Generic;

namespace AspNetCore.Mvc.Fragments
{
    public class FragmentResources
    {
        private List<string> _preScripts;
        public List<string> PreScripts => _preScripts ?? (_preScripts = new List<string>());

        private List<string> _postScripts;
        public List<string> PostScripts => _postScripts ?? (_postScripts = new List<string>());

        private List<string> _preStyles;
        public List<string> PreStyles => _preStyles ?? (_preStyles = new List<string>());

        private List<string> _postStyles;
        public List<string> PostStyles => _postStyles ?? (_postStyles = new List<string>());
    }
}