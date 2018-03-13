namespace AspNetCore.Mvc.Fragments.Resolver
{
    public interface IFragmentResolver
    {
        Fragment Resolve(string name);
    }
}
