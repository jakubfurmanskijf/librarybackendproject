using Library.Web.Models;
using Library.Web.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Web.Pages.Members;

public class CreateModel : PageModel
{
    [BindProperty] public MemberDto Member { get; set; } = new();
    public string? Error { get; set; }

    private readonly IHttpClientFactory _http;
    public CreateModel(IHttpClientFactory http) => _http = http;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        var client = ApiClientFactory.Create(HttpContext, _http);
        var resp = await client.PostAsJsonAsync("/api/Members", Member);
        if (!resp.IsSuccessStatusCode)
        {
            Error = $"Create failed: {(int)resp.StatusCode}";
            return Page();
        }
        return RedirectToPage("/Members/Index");
    }
}
