using DotnetAPIBasics.DTO;
using DotnetAPIBasics.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPIBasics.Controllers;


[ApiController]
[Route("api/[controller]")]
//http://localhost:5260/swagger/index.html
public class MiddlewareController(IDepartmentRepository departmentRepository) : ControllerBase
{
    private readonly IDepartmentRepository DepartmentRepository = departmentRepository;

    //  http://localhost:5260/api/Department/
    [HttpGet]
    public IActionResult GetAlldepartment()
    {
        return Ok(DepartmentRepository.GetAll());
    }

}
