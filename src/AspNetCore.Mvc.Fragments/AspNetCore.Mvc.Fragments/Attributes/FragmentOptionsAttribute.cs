using System;
using AspNetCore.Mvc.Fragments.Options;
using Newtonsoft.Json;

namespace AspNetCore.Mvc.Fragments.Attributes
{
    public class FragmentOptionsAttribute : Attribute, IFragmentOptions
    {
        public bool IsSync { get; set; }
        public bool IsSyncOnAjax { get; set; }
        public string PlaceHolderViewName { get; set; }

        private string[] _styles;
        public string[] Styles
        {
            get => _styles ?? (_styles = new string[0]);
            set => _styles = value;
        }

        private string[] _preScripts;
        public string[] PreScripts
        {
            get => _preScripts ?? (_preScripts = new string[0]);
            set => _preScripts = value;
        }

        private string[] _postScripts;
        public string[] PostScripts
        {
            get => _postScripts ?? (_postScripts = new string[0]);
            set => _postScripts = value;
        }

        [JsonIgnore]
        public override object TypeId => base.TypeId;
    }
}