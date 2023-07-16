namespace CorrelationId.Middlewares;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private const string _correlationHeaderString = "X-Correlation-Id";

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext ctx, ICorrelationIdManager correlationIdManager)
    {
        string correlationId = GetCorrelationId(ctx, correlationIdManager);
        AddCorrelationToHeader(ctx, correlationIdManager);
        await _next(ctx);
    }

    private static void AddCorrelationToHeader(HttpContext ctx, ICorrelationIdManager correlationIdManager)
    {
        ctx.Response.OnStarting(() =>
        {
            ctx.Response.Headers.Add(_correlationHeaderString, correlationIdManager.Get());
            return Task.CompletedTask;
        });
    }

    private static string GetCorrelationId(HttpContext ctx, ICorrelationIdManager correlationIdManager)
    {
        if (ctx.Request.Headers.TryGetValue(_correlationHeaderString, out var correlationId))
        {
            correlationIdManager.Set(correlationId);
            return correlationId;
        }
        else
        {
            return correlationIdManager.Get();
        }
    }
}
