using Library.Domain.Models;
using Library.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Controllers;

public record BorrowRequest(int MemberId, int BookId, int Days);
public record ReturnRequest(int BorrowingId);

[ApiController]
[Route("api/[controller]")]
public class BorrowingsController : ControllerBase
{
    private readonly LibraryDbContext _db;
    public BorrowingsController(LibraryDbContext db) => _db = db;

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Borrowing>>> GetAll()
        => Ok(await _db.Borrowings
            .Include(b => b.Book)
            .Include(b => b.Member)
            .AsNoTracking()
            .ToListAsync());

    [Authorize]
    [HttpPost("borrow")]
    public async Task<IActionResult> Borrow(BorrowRequest req)
    {
        var book = await _db.Books.FindAsync(req.BookId);
        var member = await _db.Members.FindAsync(req.MemberId);
        if (book is null || member is null) return NotFound();

        if (!book.TryBorrow()) return BadRequest("No copies available.");

        var br = new Borrowing
        {
            BookId = book.Id,
            MemberId = member.Id,
            BorrowedAtUtc = DateTime.UtcNow,
            DueAtUtc = DateTime.UtcNow.AddDays(req.Days <= 0 ? 14 : req.Days)
        };

        _db.Borrowings.Add(br);
        await _db.SaveChangesAsync();
        return Ok(br);
    }

    [Authorize]
    [HttpPost("return")]
    public async Task<IActionResult> Return(ReturnRequest req)
    {
        var br = await _db.Borrowings.Include(x => x.Book).FirstOrDefaultAsync(x => x.Id == req.BorrowingId);
        if (br is null) return NotFound();
        if (br.IsReturned) return BadRequest("Already returned.");

        br.ReturnedAtUtc = DateTime.UtcNow;
        br.Book!.ReturnCopy();

        await _db.SaveChangesAsync();
        return Ok(br);
    }
}
