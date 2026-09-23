using DotnetAPIBasics.Filters;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPIBasics.Controllers;
//we can applay the filter attribute on the controller or on the action or on the global level
//when we applay it on the controller it will execute for all actions in the controller
//when we applay it on the action it will execute only for that action
//to apply the filter on all controllers we can use the
//we can appay it in the program.cs file
// builder.Services.AddControllers(
//      options => options.Filters.Add(new LogActivityFilterAttribute())
//      options.Filters.Add(new HandelErorrAttribute())
// );


[ApiController]
[Route("api/[controller]")]
//[LogActivityFilter]
//[HandelErorr]
//http://localhost:5260/swagger/index.html
public class FilterController(IDepartmentRepository departmentRepository) : ControllerBase
{
    private readonly IDepartmentRepository DepartmentRepository = departmentRepository;

    //  http://localhost:5260/api/Department/

    [HttpGet]
    //[LogActivityFilter(logger: logger)]
    public IActionResult GetAlldepartment()
    {
        //throw new Exception("Something went wrong");
        return Ok(DepartmentRepository.GetAll());
    }
    [HttpGet("{id:int}")]
    [HandelErorr]
    public IActionResult GetByDepartmentId(int id)
    {
        //throw new Exception("Something went wrong");
        return Ok(DepartmentRepository.GetById(id));
    }

}