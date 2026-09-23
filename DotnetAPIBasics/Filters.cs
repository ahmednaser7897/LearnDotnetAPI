// https://learn.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-10.0

// ==========================================================
// 1. GENERAL CONCEPT
// ==========================================================

// Filters are used to execute code before or after specific
// stages in the MVC request processing pipeline.

// Filters are executed after ASP.NET Core selects the action.

// IFilterMetadata is the base marker interface for all MVC filters.
// It does NOT contain the filter methods.

// Filters can implement filter interfaces or inherit from
// filter base classes such as ActionFilterAttribute.

// ==========================================================
// 2. FILTER SCOPES
// ==========================================================

// Filters can be applied at 3 levels:

// 1. Global Level
// Apply the filter to all controllers and actions.

// In Program.cs in MVC:
// builder.Services.AddControllersWithViews(options =>
// {
//     options.Filters.Add(new HandleErrorAttribute());
// });
// In Program.cs in API:
// builder.Services.AddControllers(options =>
// {
//     options.Filters.Add(new HandleErrorAttribute());
// });


// 2. Controller Level
// Apply the filter to all actions in a controller.

// [HandleError]
// public class FilterController : Controller
// {
//     ...
// }

// 3. Action Level
// Apply the filter to a specific action.

// [HandleError]
// public IActionResult Index()
// {
//     ...
// }

// ==========================================================
// 3. FILTER EXECUTION ORDER
// ==========================================================

// Filters run in the following order:

// 1. Authorization Filters
//    Example: AuthorizeFilter

// 2. Resource Filters
//    Example: IResourceFilter

// 3. Action Filters
//    Example: IActionFilter

// 4. Exception Filters
//    Example: IExceptionFilter

// 5. Result Filters
//    Example: IResultFilter

// Note:
// Exception filters handle certain unhandled exceptions
// during MVC action execution, before result execution.

// Result filters run around the execution of action results.

// ==========================================================
// 4. FILTER INTERFACES
// ==========================================================

// IFilterMetadata:
// Base marker interface for MVC filters.

// IAuthorizationFilter:
// Runs during authorization.

// IResourceFilter:
// Runs before and after most of the MVC pipeline.

// IActionFilter:
// Runs before and after an action method.

// IExceptionFilter:
// Runs when an unhandled exception occurs in the
// supported MVC execution stages.

// IResultFilter:
// Runs before and after an action result executes.

// ==========================================================
// 5. SYNCHRONOUS FILTER METHODS
// ==========================================================

// IActionFilter:
// 1. OnActionExecuting()
//    Executes before the action method.

// 2. OnActionExecuted()
//    Executes after the action method.

// IExceptionFilter:
// 1. OnException()
//    Executes when a supported unhandled exception occurs.

// IResultFilter:
// 1. OnResultExecuting()
//    Executes before the action result.

// 2. OnResultExecuted()
//    Executes after the action result.

// ==========================================================
// 6. ASYNCHRONOUS FILTER INTERFACES
// ==========================================================

// IAsyncActionFilter
// IAsyncResourceFilter
// IAsyncExceptionFilter
// IAsyncResultFilter

// These interfaces provide asynchronous filter execution.

// ==========================================================
// 7. IMPORTANT NOTES
// ==========================================================

// 1. Not every filter must inherit from Attribute.

// 2. Filters can be registered globally, on controllers,
//    or on actions.

// 3. Global filters generally surround controller filters,
//    which surround action-level filters.

// 4. The "before" methods execute in order;
//    the "after" methods execute in reverse order.

// 5. Exception filters do not handle every application error.
//    They do not catch exceptions from middleware, routing,
//    model binding, resource filters, or result execution.

// 6. Middleware is generally preferred for application-wide
//    exception handling.

// ==========================================================
// 7. Action Filter vs Middleware
// ==========================================================

// Middleware
// - Executes for EVERY incoming HTTP request
// - Runs before MVC framework components
// - Ideal for application-wide cross-cutting concerns
// - Examples: logging, authentication, CORS, compression

// Action Filter
// - Executes only when a controller action is selected
// - Runs after routing but before action execution
// - Ideal for controller/action-specific concerns
// - Examples: input validation, logging, authorization per action

