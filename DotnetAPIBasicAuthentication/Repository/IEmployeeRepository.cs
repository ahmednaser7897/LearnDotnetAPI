using DotnetAPIBasicAuthentication.Models;
namespace DotnetAPIBasicAuthentication.Repository;

public interface IEmployeeRepository
{
    public string Id { get; set; }
    Employee? GetById(int id);
    Employee? GetByName(string name);
    List<Employee> GetAll();
    bool Add(Employee employee);
    bool Update(Employee model);
    bool Remove(int id);
    bool SaveChanges();
}
