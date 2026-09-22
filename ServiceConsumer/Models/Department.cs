namespace ServiceConsumer.Models;

public class Department
{
    public string Name { get; set; } = null!;
    public string? ManagerName { get; set; }

    public int EmpCount { get; set; }
}
