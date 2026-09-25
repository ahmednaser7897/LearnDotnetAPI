using DotnetAPIIdentityAuthentication.DTO;
using DotnetAPIIdentityAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPIIdentityAuthentication.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
//http://localhost:5260/swagger/index.html
public class EmployeeController(IEmployeeRepository EmployeeRepository) : ControllerBase
{
    private readonly IEmployeeRepository EmployeeRepository = EmployeeRepository;

    //  http://localhost:5260/api/Employee/

    [HttpGet]
    public ActionResult<GenralResponse> GetAllEmployee()
    {
        var allEmployees = EmployeeRepository.GetAll();
        var response = new GenralResponse
        {
            Data = allEmployees,
            IsSuccess = true,
            Message = "Employees retrieved successfully",
            StatusCode = 200
        };
        return Ok(response);
    }

    [HttpGet]
    [Route("{id:int}")]
    public ActionResult<GenralResponse> GetByEmployeeId(int id)
    {
        var employee = EmployeeRepository.GetById(id);
        GenralResponse response;
        if (employee != null)
        {
            response = new GenralResponse
            {
                Data = employee,
                IsSuccess = true,
                Message = "Employee retrieved successfully",
                StatusCode = 200
            };
        }
        else
        {
            response = new GenralResponse
            {
                Data = null,
                IsSuccess = false,
                Message = "Employee not found",
                StatusCode = 404
            };
        }
        return Ok(response);
    }

    [HttpGet("{name}")]
    public IActionResult GetByEmployeeName(string name)
    {
        return Ok(EmployeeRepository.GetByName(name));
    }

    [HttpPost]
    public IActionResult AdEmployee(Employee Employee)
    {
        EmployeeRepository.Add(Employee);
        EmployeeRepository.SaveChanges();
        var model = GetByEmployeeId(Employee.Id);
        //ex: location: http://localhost:5260/api/Employee/6
        return CreatedAtAction(nameof(GetByEmployeeId), new { id = Employee.Id }, model);
    }
    [HttpPut("{id:int}")]
    public IActionResult UpdateEmployee(int id, Employee Employee)
    {
        var deptFromDb = EmployeeRepository.GetById(id);
        if (deptFromDb != null)
        {
            deptFromDb.Name = Employee.Name;
            deptFromDb.Address = Employee.Address;
            deptFromDb.Salary = Employee.Salary;
            deptFromDb.JopTitle = Employee.JopTitle;
            deptFromDb.ImageUrl = Employee.ImageUrl;
            EmployeeRepository.Update(deptFromDb);
            EmployeeRepository.SaveChanges();
            return NoContent();
        }
        else
        {
            return NotFound($"Employee with id {id} is not found");
        }

    }
    [HttpDelete("{id:int}")]
    public IActionResult DeleteEmployee(int id)
    {
        var isDeleted = EmployeeRepository.Remove(id);
        if (isDeleted)
        {
            EmployeeRepository.SaveChanges();
            return NoContent();
        }
        else
        {
            return NotFound($"Employee with id {id} is not found");
        }
    }

}
