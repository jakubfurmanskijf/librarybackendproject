namespace Library.Web.Utils;

public static class ApiClientFactory
{
    public static HttpClient Create(HttpContext httpContext, IHttpClientFactory factory)
    {
        var client = factory.CreateClient("Api");
        var token = httpContext.Session.GetString("JWT");
        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Add("X-Client-Id", "web-ui");
        }
        return client;
    }
}
