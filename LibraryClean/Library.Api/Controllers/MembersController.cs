using Library.Domain.Models;
using Library.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class MembersController : ControllerBase
{
    private readonly LibraryDbContext _db;
    public MembersController(LibraryDbContext db) => _db = db;

    [Authorize] // any logged-in user
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Member>>> GetAll()
        => Ok(await _db.Members.AsNoTracking().ToListAsync());

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Member>> Create(Member member)
    {
        _db.Members.Add(member);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = member.Id }, member);
    }
}
