using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// config 
var apiBase = builder.Configuration["Api:BaseUrl"] ?? "https://localhost:7221";
var soapBase = apiBase.TrimEnd('/') + "/soap";

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

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});

builder.Services.AddHttpClient("Api", c => { c.BaseAddress = new Uri(apiBase); });

builder.Services.AddHttpClient("Soap", c => { c.BaseAddress = new Uri(soapBase); });


builder.Services.AddScoped<Library.Web.Services.JwtAuthService>();
builder.Services.AddScoped<Library.Web.Services.SoapClientService>(); 


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
