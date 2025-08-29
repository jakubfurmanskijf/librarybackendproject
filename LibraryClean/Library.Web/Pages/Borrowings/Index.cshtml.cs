using Library.Web.Models;
using Library.Web.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Web.Pages.Borrowings;

public class IndexModel : PageModel
{
    public List<BorrowingDto> Items { get; set; } = new();
    private readonly IHttpClientFactory _http;
    public IndexModel(IHttpClientFactory http) => _http = http;

    public async Task OnGet()
    {
        var client = ApiClientFactory.Create(HttpContext, _http);
        var resp = await client.GetAsync("/api/Borrowings");
        resp.EnsureSuccessStatusCode();
        Items = await resp.Content.ReadFromJsonAsync<List<BorrowingDto>>() ?? new();
    }
}
