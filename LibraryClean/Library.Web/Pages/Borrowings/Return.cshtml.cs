using Library.Web.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Web.Pages.Borrowings;

public class ReturnModel : PageModel
{
    [BindProperty] public int BorrowingId { get; set; }

    public string? Message { get; set; }
    public string? Error { get; set; }

    private readonly IHttpClientFactory _http;
    public ReturnModel(IHttpClientFactory http) => _http = http;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        var client = ApiClientFactory.Create(HttpContext, _http);
        var resp = await client.PostAsJsonAsync("/api/Borrowings/return",
            new { borrowingId = BorrowingId });

        if (!resp.IsSuccessStatusCode)
        {
            Error = $"Return failed: {(int)resp.StatusCode}";
            return Page();
        }

        Message = "Returned.";
        return RedirectToPage("/Borrowings/Index");
    }
}
