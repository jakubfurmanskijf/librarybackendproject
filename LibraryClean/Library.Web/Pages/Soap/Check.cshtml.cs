using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Library.Web.Services;

namespace Library.Web.Pages.Soap;

[Authorize]
public class CheckModel : PageModel
{
    [BindProperty] public string Isbn { get; set; } = "9780307474278";

    public string Ping { get; set; } = "";
    public bool Available { get; set; }
    public SoapClientService.SoapBookDto? Book { get; set; }
    public bool HasResult { get; set; }

    private readonly SoapClientService _soap;
    public CheckModel(SoapClientService soap) => _soap = soap;

    public void OnGet() { }

    public async Task<IActionResult> OnPost()
    {
        Ping = await _soap.PingAsync();
        Book = await _soap.GetBookByIsbnAsync(Isbn);
        Available = await _soap.IsAvailableAsync(Isbn);
        HasResult = true;
        return Page();
    }
}
