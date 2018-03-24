using System;

namespace AspNetCore.Mvc.Fragments.Demo.Services
{
    public class TokenResponse
    {
        public bool IsAuthorized { get; set; }
        public UserInfo User { get; set; }

        public class UserInfo
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
        }
    }
}