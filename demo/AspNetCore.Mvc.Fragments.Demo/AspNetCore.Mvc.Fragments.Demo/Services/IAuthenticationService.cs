using System.Threading.Tasks;

namespace AspNetCore.Mvc.Fragments.Demo.Services
{
    public interface IAuthenticationService
    {
        Task<TokenResponse> ResolveTokenAsync(string token);
    }
}