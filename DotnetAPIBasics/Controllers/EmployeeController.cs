using Microsoft.AspNetCore.Mvc;

namespace DotnetAPIBasics.Controllers;

public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository EmployeeRepository;
    private readonly IDepartmentRepository DepartmentRepository;
    public EmployeeController(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)//Injection
    {
        EmployeeRepository = employeeRepository;
        DepartmentRepository = departmentRepository;
    }

}




