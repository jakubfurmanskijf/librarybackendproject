using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Models;

public class Borrowing
{
    public int Id { get; set; }

    public int BookId { get; set; }
    public Book? Book { get; set; }

    public int MemberId { get; set; }
    public Member? Member { get; set; }

    public DateTime BorrowedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime DueAtUtc { get; set; }
    public DateTime? ReturnedAtUtc { get; set; }

    public bool IsReturned => ReturnedAtUtc.HasValue;
    public bool IsOverdue => !IsReturned && DateTime.UtcNow > DueAtUtc;
}