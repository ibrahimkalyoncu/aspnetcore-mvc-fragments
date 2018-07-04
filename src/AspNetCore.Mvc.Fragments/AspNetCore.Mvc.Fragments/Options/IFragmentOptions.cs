namespace AspNetCore.Mvc.Fragments.Options
{
    public interface IFragmentOptions
    {
        bool IsSync { get; set; }
        bool IsSyncOnAjax { get; set; }
        string PlaceHolderViewName { get; set; }
        string[] Styles { get; set; }
        string[] PreScripts { get; set; }
        string[] PostScripts { get; set; }
    }
}
