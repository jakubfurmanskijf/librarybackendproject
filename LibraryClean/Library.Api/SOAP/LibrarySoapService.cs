using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Soap;

public class LibrarySoapService : ILibrarySoapService
{
    private readonly LibraryDbContext _db;
    public LibrarySoapService(LibraryDbContext db) => _db = db;

    public string Ping() => "PONG";

    public SoapBookDto? GetBookByIsbn(string isbn)
    {
        var b = _db.Books.AsNoTracking().FirstOrDefault(x => x.Isbn == isbn);
        return b is null ? null : new SoapBookDto
        {
            Id = b.Id,
            Isbn = b.Isbn,
            Title = b.Title,
            Author = b.Author,
            Year = b.Year,
            TotalCopies = b.TotalCopies,
            AvailableCopies = b.AvailableCopies
        };
    }

    public bool IsAvailable(string isbn)
    {
        var b = _db.Books.AsNoTracking().FirstOrDefault(x => x.Isbn == isbn);
        return b is not null && b.AvailableCopies > 0;
    }
}
