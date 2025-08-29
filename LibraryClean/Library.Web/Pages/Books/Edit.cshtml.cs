using Library.Web.Models;
using Library.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Web.Pages.Books;

[Authorize(Roles = "Admin")]
public class EditModel : PageModel
{
    [BindProperty] public BookDto Book { get; set; } = new();
    public string? Error { get; set; }

    private readonly IHttpClientFactory _http;
    public EditModel(IHttpClientFactory http) => _http = http;

    public async Task<IActionResult> OnGet(int id)
    {
        var client = ApiClientFactory.Create(HttpContext, _http);
        var resp = await client.GetAsync($"/api/Books/{id}");
        if (!resp.IsSuccessStatusCode) return RedirectToPage("/Books/Index");
        Book = await resp.Content.ReadFromJsonAsync<BookDto>() ?? new();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var client = ApiClientFactory.Create(HttpContext, _http);
        var resp = await client.PutAsJsonAsync($"/api/Books/{Book.Id}", Book);
        if (!resp.IsSuccessStatusCode)
        {
            Error = $"Update failed: {(int)resp.StatusCode}";
            return Page();
        }
        return RedirectToPage("/Books/Index");
    }
}
