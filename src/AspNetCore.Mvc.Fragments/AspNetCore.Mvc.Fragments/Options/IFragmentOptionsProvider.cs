namespace AspNetCore.Mvc.Fragments.Options
{
    public interface IFragmentOptionsProvider
    {
        IFragmentOptions GetFragmentOptions(Fragment fragment);
    }
}
