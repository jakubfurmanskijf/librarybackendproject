using Library.Web.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Library.Web.Pages.Borrowings;

public class BorrowModel : PageModel
{
    [BindProperty] public int MemberId { get; set; } = 1;
    [BindProperty] public int BookId { get; set; } = 1;
    [BindProperty] public int Days { get; set; } = 14;

    public string? Message { get; set; }
    public string? Error { get; set; }

    private readonly IHttpClientFactory _http;
    public BorrowModel(IHttpClientFactory http) => _http = http;

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        var client = ApiClientFactory.Create(HttpContext, _http);
        var resp = await client.PostAsJsonAsync("/api/Borrowings/borrow",
            new { memberId = MemberId, bookId = BookId, days = Days });

        if (!resp.IsSuccessStatusCode)
        {
            Error = $"Borrow failed: {(int)resp.StatusCode}";
            return Page();
        }

        Message = "Borrowed successfully.";
        return RedirectToPage("/Borrowings/Index");
    }
}
