namespace Library.Api.Auth;

public static class FakeUsers
{
    // username -> (password, role)
    public static readonly Dictionary<string, (string Password, string Role)> Users = new()
    {
        ["admin"] = ("admin123", "Admin"),
        ["user"] = ("user123", "User")
    };
}
