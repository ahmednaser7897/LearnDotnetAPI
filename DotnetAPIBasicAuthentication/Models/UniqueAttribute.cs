

using System.ComponentModel.DataAnnotations;
using DotnetAPIBasicAuthentication.Models.Data;
namespace DotnetAPIBasicAuthentication.Models;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
public class UniqueAttribute : ValidationAttribute
{
    // if it is add or update request
    // we need to check if the name already exists
    // and if it is the same employee we are updating
    // we should not return an error
    // if it is a new employee
    // we should return an error if the name already exists
    //validationContext.ObjectInstance holdes the entire employee object
    public string? Message { get; set; }
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // if (value == null)
        // {
        //     return null;
        // }
        // string name = value.ToString() ?? "";
        // var employeeFromDB = new EmployeeRepository().GetByName(name);
        // var employeeFromView = (Employee)validationContext.ObjectInstance;
        // if (employeeFromDB != null)
        // {
        //     return new ValidationResult(ErrorMessage ?? "Name already exists");
        // }
        // else
        // {
        return ValidationResult.Success;
        //}
    }
}
