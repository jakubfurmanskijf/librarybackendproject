using System.Text.Json.Serialization;

namespace Library.Domain.Models;

public class Book
{
    public int Id { get; set; }
    public string Isbn { get; set; } = "";
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public int Year { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }

    [JsonIgnore]  // <-- prevents Book -> Borrowings -> Book -> ... loop
    public ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();

    public bool TryBorrow()
    {
        if (AvailableCopies <= 0) return false;
        AvailableCopies--;
        return true;
    }

    public void ReturnCopy()
    {
        if (AvailableCopies < TotalCopies) AvailableCopies++;
    }
}
