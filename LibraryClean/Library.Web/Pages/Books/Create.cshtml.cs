using Library.Web.Models;
using Library.Web.Utils;
using Microsoft.AspNetCore.Authorization;    // <—
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Web.Pages.Books;

[Authorize(Roles = "Admin")]                  // <—
public class CreateModel : PageModel
{
    [BindProperty] public BookDto Book { get; set; } = new();
    public string? Error { get; set; }
    private readonly IHttpClientFactory _http;
    public CreateModel(IHttpClientFactory http) => _http = http;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        var client = ApiClientFactory.Create(HttpContext, _http);
        var resp = await client.PostAsJsonAsync("/api/Books", Book);
        if (!resp.IsSuccessStatusCode)
        {
            Error = $"Create failed: {(int)resp.StatusCode}";
            return Page();
        }
        return RedirectToPage("/Books/Index");
    }
}
