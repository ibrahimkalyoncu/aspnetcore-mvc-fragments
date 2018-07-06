using System.Threading.Tasks;
using AspNetCore.Mvc.Fragments.Attributes;
using AspNetCore.Mvc.Fragments.Demo.Services;

namespace AspNetCore.Mvc.Fragments.Demo.Fragments.Header
{
    [FragmentOptions(
        PlaceHolderViewName = "/Views/Fragments/Header/PlaceHolder.cshtml", 
        PostScripts = new[] { "scripts/fragments/header.js" }, 
        Styles = new[] { "styles/fragments.css", "styles/fragments/header.css", "https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" })]
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
            return View("/Views/Fragments/Header/Index.cshtml", new HeaderFragmentViewModel
            {
                IsAuthorized = tokenResponse.IsAuthorized,
                UserDisplayName = tokenResponse.User?.Name,
                UserEmail = tokenResponse.User?.Email
            });
        }
    }
}
