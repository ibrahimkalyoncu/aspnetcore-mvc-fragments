using System.Threading.Tasks;
using AspNetCore.Mvc.Fragments.Attributes;
using AspNetCore.Mvc.Fragments.Demo.Services;

namespace AspNetCore.Mvc.Fragments.Demo.Fragments.Main
{
    [FragmentOptions(PlaceHolderViewName = "Fragments/Main/PlaceHolder", Styles = new[] { "/styles/main.css" })]
    public class MainFragment : Fragment
    {
        private readonly IContentService _contentService;

        public MainFragment(IContentService contentService)
        {
            _contentService = contentService;
        }

        public async Task<FragmentViewResult> ProcessAsync()
        {
            return View("Fragments/Main/Index", new MainFragmentViewModel
            {
                Content = await _contentService.GetMainContentAsync()
            });
        }
    }
}
