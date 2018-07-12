using System;
using System.Threading.Tasks;

namespace AspNetCore.Mvc.Fragments.Demo.FragmentSource1.Services
{
    class AuthenticationService : IAuthenticationService
    {
        public async Task<TokenResponse> ResolveTokenAsync(string token)
        {
            var randomValue = new Random().Next(750, 1000);
            await Task.Delay(randomValue);
            return new TokenResponse
            {
                IsAuthorized = randomValue > 3000,
                User = randomValue > 3000 ? new TokenResponse.UserInfo { Name = "Halil Ibrahim Kalyoncu", Email = "ibrahimkalyoncu@hotmail.com.tr" } : null
            };
        }
    }
}