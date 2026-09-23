namespace DotnetAPIBasics.Middlewares;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private const int MaxRequestCount = 5;
    private static int CurrentRequestCount = 0;
    private static DateTime LastRequestTime = DateTime.Now;
    public RateLimitingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        CurrentRequestCount++;
        if (DateTime.Now.Subtract(LastRequestTime).TotalSeconds > 10)
        {
            CurrentRequestCount = 1;
            LastRequestTime = DateTime.Now;
            await _next(context);
        }
        else
        {
            if (CurrentRequestCount > MaxRequestCount)
            {
                context.Response.StatusCode = 429;
                LastRequestTime = DateTime.Now;
                await context.Response.WriteAsync("Too many requests");
            }
            else
            {
                LastRequestTime = DateTime.Now;
                await _next(context);
            }
        }
    }
}