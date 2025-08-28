using System.Security.Claims;
using Library.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Web.Pages.Account;

public class LoginModel : PageModel
{
    public record LoginInput(string Username, string Password);
    [BindProperty] public LoginInput Input { get; set; } = new("", "");
    public string? Error { get; set; }

    private readonly JwtAuthService _auth;

    public LoginModel(JwtAuthService auth) => _auth = auth;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        var (ok, token, principal, error) = await _auth.LoginAsync(Input.Username, Input.Password);
        if (!ok || token is null || principal is null)
        {
            Error = error ?? "Invalid credentials";
            return Page();
        }

        // Store JWT in session for API calls
        HttpContext.Session.SetString("JWT", token);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(6)
            });

        return RedirectToPage("/Index");
    }
}
