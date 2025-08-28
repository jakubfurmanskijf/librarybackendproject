using Library.Domain.Models;
using Library.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly LibraryDbContext _db;
    public BooksController(LibraryDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetAll()
        => Ok(await _db.Books.AsNoTracking().ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Book>> Get(int id)
    {
        var b = await _db.Books.FindAsync(id);
        return b is null ? NotFound() : Ok(b);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Book>> Create(Book book)
    {
        _db.Books.Add(book);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Book book)
    {
        if (id != book.Id) return BadRequest();
        _db.Entry(book).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var b = await _db.Books.FindAsync(id);
        if (b is null) return NotFound();
        _db.Books.Remove(b);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
