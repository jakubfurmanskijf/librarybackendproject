using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Library.Web.Utils; // ApiClientFactory

namespace Library.Web.Pages.Borrowings;

public class BorrowModel : PageModel
{
    // Selected values
    [BindProperty] public int MemberId { get; set; }
    [BindProperty] public int BookId { get; set; }
    [BindProperty] public int Days { get; set; } = 14;

    // Display messages
    public string? Message { get; set; }
    public string? Error { get; set; }

    // Dropdown options
    public List<SelectListItem> MemberOptions { get; set; } = new();
    public List<SelectListItem> BookOptions { get; set; } = new();

    private readonly IHttpClientFactory _http;
    public BorrowModel(IHttpClientFactory http) => _http = http;

    private record MemberDto(int Id, string FullName, string Email);
    private record BookDto(int Id, string Title, string Author, int TotalCopies, int AvailableCopies);

    public async Task<IActionResult> OnGet()
    {
        return await LoadListsAndShowAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var client = ApiClientFactory.Create(HttpContext, _http);
        var payload = new { memberId = MemberId, bookId = BookId, days = Days };

        var resp = await client.PostAsJsonAsync("/api/Borrowings/borrow", payload);

        if (resp.StatusCode == HttpStatusCode.Unauthorized) return Challenge();
        if (resp.StatusCode == HttpStatusCode.Forbidden) return Forbid();

        if (!resp.IsSuccessStatusCode)
        {
            Error = resp.StatusCode switch
            {
                HttpStatusCode.NotFound => "Member or book not found.",
                HttpStatusCode.Conflict => "No copies available to borrow.",
                HttpStatusCode.BadRequest => "Invalid request.",
                _ => $"Borrow failed: {(int)resp.StatusCode}"
            };

            return await LoadListsAndShowAsync();
        }

        Message = "Borrowed successfully.";
        return RedirectToPage("/Borrowings/Index");
    }

    private async Task<IActionResult> LoadListsAndShowAsync()
    {
        var client = ApiClientFactory.Create(HttpContext, _http);

        var mResp = await client.GetAsync("/api/Members");
        if (mResp.StatusCode == HttpStatusCode.Unauthorized) return Challenge();
        if (mResp.StatusCode == HttpStatusCode.Forbidden) return Forbid();

        var members = await mResp.Content.ReadFromJsonAsync<List<MemberDto>>() ?? new();
        MemberOptions = members
            .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.FullName })
            .ToList();

        if (MemberOptions.Count > 0 && MemberId == 0)
            MemberId = int.Parse(MemberOptions[0].Value);

        var bResp = await client.GetAsync("/api/Books");
        if (bResp.StatusCode == HttpStatusCode.Unauthorized) return Challenge();
        if (bResp.StatusCode == HttpStatusCode.Forbidden) return Forbid();

        var books = await bResp.Content.ReadFromJsonAsync<List<BookDto>>() ?? new();
        BookOptions = books
            .Where(b => b.AvailableCopies > 0)
            .Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = $"{b.Title} — {b.AvailableCopies}/{b.TotalCopies} left"
            })
            .ToList();

        if (BookOptions.Count > 0 && BookId == 0)
            BookId = int.Parse(BookOptions[0].Value);

        return Page();
    }
}
