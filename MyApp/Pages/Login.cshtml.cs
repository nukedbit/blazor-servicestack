using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Pages
{
    public class Login : PageModel
    {
        public IActionResult OnGet()
        {
            if (User?.Identity?.IsAuthenticated ?? false)
            {
                var r = Redirect ?? "/";
                if (r.EndsWith("logout"))
                {
                    r = "/";
                }
                return LocalRedirect(r);
            }
            return Page();
        }

        [BindProperty(SupportsGet = true)]
        public string Redirect { get; set; }
    }
}