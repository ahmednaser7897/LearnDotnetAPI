using DotnetAPIBasicAuthentication.DTO;
using DotnetAPIBasicAuthentication.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPIBasicAuthentication.Controllers;

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

    [HttpGet()]
    public IActionResult GetAlldepartment()
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


    [HttpGet]
    [Route("{id:int}")]//api/department/"id"
    public IActionResult GetByDepartmentId(int id)
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




}
