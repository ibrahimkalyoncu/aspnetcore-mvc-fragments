using System;
using System.Threading.Tasks;

namespace AspNetCore.Mvc.Fragments.Demo.Services
{
    class AuthenticationService : IAuthenticationService
    {
        public async Task<TokenResponse> ResolveTokenAsync(string token)
        {
            var randomValue = new Random().Next(1000, 5000);
            await Task.Delay(randomValue);
            return new TokenResponse
            {
                IsAuthorized = randomValue > 3000,
                User = randomValue > 3000 ? new TokenResponse.UserInfo { Name = "Halil Ibrahim Kalyoncu", Email = "ibrahimkalyoncu@hotmail.com.tr" } : null
            };
        }
    }
}