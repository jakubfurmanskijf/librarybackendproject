using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Web.Pages.Account;

public class LogoutModel : PageModel
{
    public async Task OnGet()
    {
        HttpContext.Session.Remove("JWT");
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        Response.Redirect("/Account/Login");
    }
}
