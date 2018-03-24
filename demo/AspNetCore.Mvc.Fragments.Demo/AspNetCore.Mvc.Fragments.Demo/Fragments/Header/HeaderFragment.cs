﻿using System.Threading.Tasks;
using AspNetCore.Mvc.Fragments.Attributes;
using AspNetCore.Mvc.Fragments.Demo.Services;

namespace AspNetCore.Mvc.Fragments.Demo.Fragments.Header
{
    [FragmentOptions(PlaceHolderViewName = "Fragments/Header/PlaceHolder", PostScripts = new[] { "scripts/fragments/header.js" }, Styles = new[] { "styles/fragments.css", "styles/fragments/header.css" })]
    public class HeaderFragment : Fragment
    {
        private readonly IAuthenticationService _authenticationService;

        public HeaderFragment(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<FragmentViewResult> ProcessAsync(HeaderFragmentModel model)
        {
            var tokenResponse = await _authenticationService.ResolveTokenAsync(model.Token);
            return View("Fragments/Header/Index", new HeaderFragmentViewModel
            {
                IsAuthorized = tokenResponse.IsAuthorized,
                UserDisplayName = tokenResponse.User?.Name,
                UserEmail = tokenResponse.User?.Email
            });
        }
    }
}