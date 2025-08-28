using System.IdentityModel.Tokens.Jwt;   // <—
using System.Net.Http.Json;              // <—
using System.Security.Claims;

namespace Library.Web.Services;

public class JwtAuthService
{
    private readonly IHttpClientFactory _http;

    public JwtAuthService(IHttpClientFactory http) => _http = http;

    public async Task<(bool ok, string? token, ClaimsPrincipal? principal, string? error)>
        LoginAsync(string username, string password)
    {
        var client = _http.CreateClient("Api");
        var resp = await client.PostAsJsonAsync("/api/Auth/login", new { username, password });
        if (!resp.IsSuccessStatusCode)
        {
            return (false, null, null, $"Login failed: {(int)resp.StatusCode}");
        }

        var token = await resp.Content.ReadAsStringAsync();

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);  // requires System.IdentityModel.Tokens.Jwt

        var name = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
                   ?? jwt.Claims.FirstOrDefault(c => c.Type.EndsWith("/name"))?.Value
                   ?? username;

        var role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
                   ?? jwt.Claims.FirstOrDefault(c => c.Type.EndsWith("/role"))?.Value
                   ?? "User";

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, name),
            new Claim(ClaimTypes.Role, role),
        }, "Cookies");

        var principal = new ClaimsPrincipal(identity);
        return (true, token, principal, null);
    }
}
