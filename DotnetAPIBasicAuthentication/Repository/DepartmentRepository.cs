using DotnetAPIBasicAuthentication.Models;
using DotnetAPIBasicAuthentication.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPIBasicAuthentication.Repository;

public class DepartmentRepository : IDepartmentRepository
{
    public string Id { get; set; }
    readonly AppDbContext context;
    public DepartmentRepository(AppDbContext context)
    {
        Id = Guid.NewGuid().ToString();
        this.context = context; //new AppDbContext();
    }

    public Department? GetById(int id)
    {
        return context.Departments.Include(e => e.Employees).FirstOrDefault(e => e.Id == id);
    }
    public Department? GetByName(string name)
    {
        return context.Departments.FirstOrDefault(e => e.Name == name);
    }

    public List<Department> GetAll()
    {
        return context.Departments
        .Include(e => e.Employees)
        .ToList();
    }

    public bool Add(Department Department)
    {
        try
        {
            context.Departments.Add(Department);
            //context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool Update(Department Department)
    {
        try
        {
            context.Departments.Update(Department);
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
        Department? model = GetById(id);
        if (model != null)
        {
            context.Departments.Remove(model);
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




