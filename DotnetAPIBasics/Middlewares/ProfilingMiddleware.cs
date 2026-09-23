using System.Diagnostics;

namespace DotnetAPIBasics.Middlewares;
//To create middleware using class we need to create a class that has a constructor that accepts RequestDelegate 
//and a method Invoke that accepts HttpContext
//we can also use extension method to create middleware using class

// we can inject any service in constructor of middleware and that will be injected by the
//dependency injection container
// in this case this middleware must be the first or second middleware in the pipeline
// because we want to profile the whole request
public class ProfilingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ProfilingMiddleware> _logger;
    public ProfilingMiddleware(RequestDelegate next, ILogger<ProfilingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task Invoke(HttpContext context)
    {
        //start
        var watch = new Stopwatch();
        watch.Start();

        await _next(context);
        //end
        watch.Stop();
        _logger.LogInformation($"Request {context.Request.Method} {context.Request.Path} took {watch.ElapsedMilliseconds} ms");

        //context.Response.Headers.Append("X-Processing-Time", watch.ElapsedMilliseconds.ToString());

    }
}
