using DotnetAPIBasics.Models;
using DotnetAPIBasics.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPIBasics.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    public string Id { get; set; }
    readonly AppDbContext context;
    public EmployeeRepository(AppDbContext context)
    {
        Id = Guid.NewGuid().ToString();
        this.context = context;//new AppDbContext();
    }
    public Employee? GetById(int id)
    {
        return context.Employees.Find(id);
    }
    public Employee? GetByName(string name)
    {
        return context.Employees.FirstOrDefault(e => e.Name == name);
    }

    public List<Employee> GetAll()
    {
        return context.Employees.Include(e => e.Department).ToList();
    }

    public bool Add(Employee employee)
    {
        try
        {
            context.Employees.Add(employee);
            //context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool Update(Employee model)
    {
        try
        {
            context.Employees.Update(model);
            //context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

    }
    public bool Remove(int id)
    {
        Employee? model = GetById(id);
        if (model != null)
        {
            context.Employees.Remove(model);
            //context.SaveChanges();
            return true;
        }
        return false;
    }
    public bool SaveChanges()
    {
        return context.SaveChanges() > 0;
    }
}




