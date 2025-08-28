using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// --- Config ---
var apiBase = builder.Configuration["Api:BaseUrl"] ?? "https://localhost:7221";

// --- Auth: Cookie for the web app (UI) ---
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/Account/Login";
        o.LogoutPath = "/Account/Logout";
        o.AccessDeniedPath = "/Account/Denied";
        o.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(); // we’ll use [Authorize] and roles

// --- Session (to store JWT token) ---
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});

// --- HttpClient to call your API ---
builder.Services.AddHttpClient("Api", c =>
{
    c.BaseAddress = new Uri(apiBase);
});

builder.Services.AddScoped<Library.Web.Services.JwtAuthService>();


// --- Razor Pages ---
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
