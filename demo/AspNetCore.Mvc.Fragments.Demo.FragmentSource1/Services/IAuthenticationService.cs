using System.Threading.Tasks;

namespace AspNetCore.Mvc.Fragments.Demo.FragmentSource1.Services
{
    public interface IAuthenticationService
    {
        Task<TokenResponse> ResolveTokenAsync(string token);
    }
}