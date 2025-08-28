using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Data;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Borrowing> Borrowings => Set<Borrowing>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Borrowings)
            .WithOne(br => br.Book!)
            .HasForeignKey(br => br.BookId)
            .OnDelete(DeleteBehavior.Cascade);  

        modelBuilder.Entity<Member>()
            .HasMany(m => m.Borrowings)
            .WithOne(br => br.Member!)
            .HasForeignKey(br => br.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed a couple of records for demo
        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, Isbn = "9780307474278", Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Year = 1925, TotalCopies = 3, AvailableCopies = 3 },
            new Book { Id = 2, Isbn = "9780061120084", Title = "To Kill a Mockingbird", Author = "Harper Lee", Year = 1960, TotalCopies = 2, AvailableCopies = 2 }
        );

        modelBuilder.Entity<Member>().HasData(
            new Member { Id = 1, FullName = "Alice Example", Email = "alice@example.com" }
        );
    }
}
