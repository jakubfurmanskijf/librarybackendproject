using Library.Web.Models;
using Library.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Web.Pages.Books;

[Authorize(Roles = "Admin")]
public class DeleteModel : PageModel
{
    [BindProperty] public int Id { get; set; }
    public BookDto? Book { get; set; }
    public string? Error { get; set; }

    private readonly IHttpClientFactory _http;
    public DeleteModel(IHttpClientFactory http) => _http = http;

    public async Task<IActionResult> OnGet(int id)
    {
        Id = id;
        var client = ApiClientFactory.Create(HttpContext, _http);
        var resp = await client.GetAsync($"/api/Books/{id}");
        if (!resp.IsSuccessStatusCode) return RedirectToPage("/Books/Index");
        Book = await resp.Content.ReadFromJsonAsync<BookDto>();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var client = ApiClientFactory.Create(HttpContext, _http);
        var resp = await client.DeleteAsync($"/api/Books/{Id}");
        if (!resp.IsSuccessStatusCode)
        {
            Error = $"Delete failed: {(int)resp.StatusCode}";
            return Page();
        }
        return RedirectToPage("/Books/Index");
    }
}
