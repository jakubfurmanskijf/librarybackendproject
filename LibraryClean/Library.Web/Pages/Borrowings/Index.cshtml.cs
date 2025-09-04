using Library.Web.Models;
using Library.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace Library.Web.Pages.Borrowings;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    public List<BorrowingDto> Items { get; set; } = new();
    private readonly IHttpClientFactory _http;
    public IndexModel(IHttpClientFactory http) => _http = http;

    public async Task<IActionResult> OnGet()
    {
        var client = ApiClientFactory.Create(HttpContext, _http);
        var resp = await client.GetAsync("/api/Borrowings");

        if (resp.StatusCode == HttpStatusCode.Unauthorized)
            return Challenge(); 

        if (resp.StatusCode == HttpStatusCode.Forbidden)
            return Forbid();     

        if (!resp.IsSuccessStatusCode)
        {
            Items = new();
            return Page();       
        }

        Items = await resp.Content.ReadFromJsonAsync<List<BorrowingDto>>() ?? new();
        return Page();
    }
}
