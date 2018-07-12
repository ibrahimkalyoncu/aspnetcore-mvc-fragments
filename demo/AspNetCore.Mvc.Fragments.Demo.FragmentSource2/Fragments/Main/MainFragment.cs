using System.Threading.Tasks;
using AspNetCore.Mvc.Fragments.Attributes;
using AspNetCore.Mvc.Fragments.Demo.FragmentSource2.Services;

namespace AspNetCore.Mvc.Fragments.Demo.FragmentSource2.Fragments.Main
{
    [FragmentOptions(
        PlaceHolderViewName = "/Views/Fragments/Main/PlaceHolder.cshtml", 
        Styles = new[] { "/styles/main.css", "https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" })]
    public class MainFragment : Fragment
    {
        private readonly IContentService _contentService;

        public MainFragment(IContentService contentService)
        {
            _contentService = contentService;
        }

        public async Task<FragmentViewResult> ProcessAsync()
        {
            return View("/Views/Fragments/Main/Index.cshtml", new MainFragmentViewModel
            {
                Content = await _contentService.GetMainContentAsync()
            });
        }
    }
}
