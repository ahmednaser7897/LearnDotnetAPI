using DotnetAPIBasics.Models;
namespace DotnetAPIBasics.Repository;

public interface IDepartmentRepository
{
    public string Id { get; set; }
    Department? GetById(int id);
    Department? GetByName(string name);
    List<Department> GetAll();
    bool Add(Department department);
    bool Update(Department model);
    bool Remove(int id);
    bool SaveChanges();
}
