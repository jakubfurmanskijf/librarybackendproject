namespace Library.Api.Middleware;

public class HeaderMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HeaderMiddleware> _logger;

    public HeaderMiddleware(RequestDelegate next, ILogger<HeaderMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext ctx)
    {
        var clientId = ctx.Request.Headers["X-Client-Id"].ToString();
        if (!string.IsNullOrWhiteSpace(clientId))
            _logger.LogInformation("X-Client-Id: {ClientId}", clientId);

        var correlationId = ctx.Request.Headers["X-Correlation-Id"].ToString();
        if (string.IsNullOrWhiteSpace(correlationId))
            correlationId = Guid.NewGuid().ToString();

        ctx.Response.Headers["X-Correlation-Id"] = correlationId;

        await _next(ctx);
    }
}
