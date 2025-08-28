using Library.Web.Models;
using Library.Web.Utils;
using Microsoft.AspNetCore.Authorization;   // <—
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Web.Pages.Books;

[Authorize]                                  // <—
public class IndexModel : PageModel
{
    public List<BookDto> Books { get; set; } = new();
    private readonly IHttpClientFactory _http;
    public IndexModel(IHttpClientFactory http) => _http = http;

    public async Task OnGet()
    {
        var client = ApiClientFactory.Create(HttpContext, _http);
        var resp = await client.GetAsync("/api/Books");
        resp.EnsureSuccessStatusCode();
        Books = await resp.Content.ReadFromJsonAsync<List<BookDto>>() ?? new();
    }
}
