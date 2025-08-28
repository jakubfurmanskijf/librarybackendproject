using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Library.Domain.Models;

public class Member
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";

    [JsonIgnore]  // <-- prevents Member -> Borrowings -> Member -> ... loop
    public ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
}
