using DotnetAPIBasics.DTO;
using DotnetAPIBasics.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPIBasics.Controllers;

//ApiController Attribute: it tells the asp.net core framework that this controller is a controller and 
// it will return JSON objects rather than views.
// and it markes the binding and validation  is diffrent from mvc controllers.
// if we did not add it it see this controller as normal mvc controller.
// Route Attribute :
// it teels us how to access this controller apis 
// here it means that the url will be localhost:5074/api/Department/
// it will know which action method we want to access based on the following info:
// 1. HTTP method (like GET, POST, PUT, DELETE)
// 2. route parameters, 
// 3. query string, 
// 4. headers
// after this url will be routed to this controller.
// so if we want to access the get method we have to use 
// localhost:5074/api/Department/get

[ApiController]
[Route("api/[controller]")]
//http://localhost:5260/swagger/index.html
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentRepository DepartmentRepository;
    public DepartmentController(IDepartmentRepository departmentRepository)
    {
        DepartmentRepository = departmentRepository;
    }

    //  http://localhost:5260/api/Department/
    [HttpGet]
    public IActionResult GetAlldepartment()
    {
        return Ok(DepartmentRepository.GetAll());
    }

    // the base route = http://localhost:5260/api/Department/
    // so it has 2 segment:
    // 1. api
    // 2. Department
    // if we idded id as a 3rd segment it will case erorr
    // so we will customize the route template for this only using [Route("{id}")] attribute.
    // so this is for the route parameter http://localhost:5260/api/Department/1
    [HttpGet]
    [Route("{id:int}")]//api/department/"id"
    public IActionResult GetByDepartmentId(int id)
    {
        return Ok(DepartmentRepository.GetById(id));
    }
    //we alse can pass it to HttpGet Attribute without using [Route("{name:string}")] Attribute
    // instead of [Route("{name:string}")]
    [HttpGet("{name}")]
    public IActionResult GetByDepartmentName(string name)
    {
        return Ok(DepartmentRepository.GetByName(name));
    }

    [HttpPost]
    public IActionResult Addepartment(Department department)
    {
        DepartmentRepository.Add(department);
        DepartmentRepository.SaveChanges();
        var model = GetByDepartmentId(department.Id);
        //return Created($"~/api/Department/{department.Id}", model);
        //we use this to add location (to Response headers)
        //that helps us to get this new department.
        //so it will return 
        //ex: location: http://localhost:5260/api/Department/6
        return CreatedAtAction(nameof(GetByDepartmentId), new { id = department.Id }, model);
    }
    [HttpPut("{id:int}")]
    public IActionResult UpdateDepartment(int id, Department department)
    {
        var deptFromDb = DepartmentRepository.GetById(id);
        if (deptFromDb != null)
        {
            deptFromDb.Name = department.Name;
            deptFromDb.ManagerName = department.ManagerName;
            DepartmentRepository.Update(deptFromDb);
            DepartmentRepository.SaveChanges();
            return NoContent();
        }
        else
        {
            return NotFound($"department with id {id} is not found");
        }

    }
    [HttpDelete("{id:int}")]
    public IActionResult DeleteDepartment(int id)
    {
        var isDeleted = DepartmentRepository.Remove(id);
        if (isDeleted)
        {
            DepartmentRepository.SaveChanges();
            return NoContent();
        }
        else
        {
            return NotFound($"department with id {id} is not found");
        }
    }
    // DTO => Data Transfer Object
    // why using DTO??
    // 1- to hide the internal structure of the database from the client
    //    ex: if we have Employee table with 100 columns and we want to send only 5 columns to the client
    //    we will create a DTO with only 5 columns and send it to the client
    // 2- to map the data from the database to the client 
    //    ex: we have Employee table with FirstName, LastName, HireDate and we want to send to the client 
    //    with FullName and Age
    //    FullName = FirstName + " " + LastName
    //    Age = DateTime.Now.Year - HireDate.Year
    // 3- to add new feilds that are not present in the model
    //    ex: in Department entity we have 3 properties and we want to send to the client with 4 properties
    //    we will create a DTO with 4 properties and send it to the client
    // 4- to remove fields that are not needed in the response
    //    ex: we have Employee table with FirstName, LastName, HireDate and we want to send to the client 
    //    with FullName and Age
    // 5- if there is many-to-many or one-to-many relations between entities
    //    ex: in Employee entity we have many-to-many relation with Department entity
    //    we will create a DTO with only department name and send it to the client
    // 6- To avoid the circular reference error
    //    ex: in Employee entity we have many-to-many relation with Department entity
    //    we will create a DTO with only department name and send it to the client
    // 7- To Decouple the API from the Database Model
    //    ex: in Employee entity we have many-to-many relation with Department entity
    //    we will create a DTO with only department name and send it to the client
    [HttpGet("DTO")]
    public IActionResult GetAlldepartmentWithDTO()
    {
        var departments = DepartmentRepository.GetAll();
        var dept = departments.Select(
            (s) => new DeptWithEmpDTO
            {
                Name = s.Name,
                ManagerName = s.ManagerName,
                EmpCount = s.Employees?.Count ?? 0
            });
        return Ok(dept);

    }
    [HttpGet("DTO/{id:int}")]
    public IActionResult GetByDepartmentIdDTO(int id)
    {
        var department = DepartmentRepository.GetById(id);
        var dept = new DeptWithEmpDTO
        {
            Name = department?.Name ?? "",
            ManagerName = department?.ManagerName ?? "",
            EmpCount = department?.Employees?.Count ?? 0
        };
        return Ok(dept);
    }

}
