using Library.Web.Models;
using Library.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace Library.Web.Pages.Members;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    public List<MemberDto> Members { get; set; } = new();
    private readonly IHttpClientFactory _http;
    public IndexModel(IHttpClientFactory http) => _http = http;

    public async Task<IActionResult> OnGet()
    {
        var client = ApiClientFactory.Create(HttpContext, _http);
        var resp = await client.GetAsync("/api/Members");

        // Not logged in -> go to Login
        if (resp.StatusCode == HttpStatusCode.Unauthorized)
            return Challenge();         // cookie auth will redirect to /Account/Login with returnUrl

        // Logged in but not allowed -> go to AccessDenied
        if (resp.StatusCode == HttpStatusCode.Forbidden)
            return Forbid();            // cookie auth will redirect to AccessDeniedPath

        // Any other non-success: show empty list instead of throwing
        if (!resp.IsSuccessStatusCode)
        {
            Members = new();
            return Page();
        }

        Members = await resp.Content.ReadFromJsonAsync<List<MemberDto>>() ?? new();
        return Page();
    }
}
