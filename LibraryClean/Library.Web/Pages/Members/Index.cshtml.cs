using Library.Web.Models;
using Library.Web.Utils;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Web.Pages.Members;

public class IndexModel : PageModel
{
    public List<MemberDto> Members { get; set; } = new();
    private readonly IHttpClientFactory _http;
    public IndexModel(IHttpClientFactory http) => _http = http;

    public async Task OnGet()
    {
        var client = ApiClientFactory.Create(HttpContext, _http);
        var resp = await client.GetAsync("/api/Members");
        resp.EnsureSuccessStatusCode();
        Members = await resp.Content.ReadFromJsonAsync<List<MemberDto>>() ?? new();
    }
}
