namespace Library.Web.Models;

public class BookDto
{
    public int Id { get; set; }
    public string Isbn { get; set; } = "";
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public int Year { get; set; }
    public int TotalCopies { get; set; }
    public int AvailableCopies { get; set; }
}
