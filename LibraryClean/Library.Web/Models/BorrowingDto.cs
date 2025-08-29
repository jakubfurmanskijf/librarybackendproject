namespace Library.Web.Models;

public class BorrowingDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public BookDto? Book { get; set; }
    public int MemberId { get; set; }
    public MemberDto? Member { get; set; }
    public DateTime BorrowedAtUtc { get; set; }
    public DateTime DueAtUtc { get; set; }
    public DateTime? ReturnedAtUtc { get; set; }
    public bool IsReturned { get; set; }
    public bool IsOverdue { get; set; }
}
