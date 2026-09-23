using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotnetAPIBasics.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class LogActivityFilterAttribute(ILogger<LogActivityFilterAttribute> logger) : Attribute, IActionFilter // this inherits from IFilterMetadata
{
    private readonly ILogger<LogActivityFilterAttribute> _logger = logger;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("LogActivityFilterAttribute: OnActionExecuting");
        _logger.LogInformation($"Action Controller: {context.Controller}");
        _logger.LogInformation($"Action name: {context.ActionDescriptor.DisplayName}");
        _logger.LogInformation($"Action method: {context.HttpContext.Request.Method}");
        _logger.LogInformation($"Action Result: {context.Result}");
        _logger.LogInformation($"Action route info: {context.ActionDescriptor.RouteValues}");
        _logger.LogInformation($"Action Arguments: {JsonSerializer.Serialize(context.ActionArguments)}");
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("LogActivityFilterAttribute: OnActionExecuted");
        _logger.LogInformation($"Action Controller: {context.Controller}");
        _logger.LogInformation($"Action name: {context.ActionDescriptor.DisplayName}");
        _logger.LogInformation($"Action method: {context.HttpContext.Request.Method}");
        _logger.LogInformation($"Action Result: {context.Result}");
        _logger.LogInformation($"Action route info: {context.ActionDescriptor.RouteValues}");
    }



    public void OnException(ExceptionContext context)
    {
        //throw new NotImplementedException();
    }
}
