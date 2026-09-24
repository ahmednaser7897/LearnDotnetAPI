using Microsoft.AspNetCore.Mvc;

namespace DotnetAPIBasics.Controllers;

/*

    Logging in ASP.NET Core
    -----------------------
    1- is a way to record events that happen during the execution of an application
    2- it has many categories or levels
        - Trace : is the lowest level of logging and is used for tracing the execution of the application
        - Debug : is used for debugging the application
        - Information : is used for logging information about the application
        - Warning : is used for logging warnings about the application(case some feature is not working as expected)
        - Error : is used for logging errors in the application(case some feature  failed)
        - Critical : is used for logging critical errors in the application(case the system crash)
        - The order of the levels is from the lowest to the highest
            so if we set the level to Information, it will log Information, Warning, Error, and Critical
            but it will not log Trace and Debug
    3- we can configure logging in the appsettings.json file
        - we can configure logging in the Program.cs file
        - we can configure logging in the controller file
    4- we can change the category of logging in the controller file like this:
        - _logger.LogInformation(message,args); 
        - _logger.LogError(message,args);
        - _logger.LogCritical(message,args);
        - _logger.LogWarning(message,args);
        - _logger.LogDebug(message,args);
    5- we can change the logger place (console window,file,database,debug window)
*/

[ApiController]
[Route("api/[controller]")]
//http://localhost:5260/swagger/index.html
public class LoggerController : ControllerBase
{
    private readonly IDepartmentRepository DepartmentRepository;
    private readonly ILogger<LoggerController> _logger;
    public LoggerController(IDepartmentRepository departmentRepository, ILogger<LoggerController> logger)
    {
        DepartmentRepository = departmentRepository;
        _logger = logger;
    }

    //  http://localhost:5260/api/Department/
    [HttpGet]
    public IActionResult GetAlldepartment()
    {
        return Ok(DepartmentRepository.GetAll());
    }


    [HttpGet("{id:int}")]
    public IActionResult GetByDepartmentId(int id)
    {
        _logger.LogDebug("Getting department by id #{id}", id);
        var dept = DepartmentRepository.GetById(id);
        if (dept == null)
        {
            _logger.LogWarning("Department with id #{id} not found", id);
            return NotFound($"department with id {id} is not found");
        }
        _logger.LogWarning("Department with id #{id} found", id);
        return Ok(dept);
    }
}
