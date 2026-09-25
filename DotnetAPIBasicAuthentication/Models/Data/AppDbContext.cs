using Microsoft.EntityFrameworkCore;
namespace DotnetAPIBasicAuthentication.Models.Data;

public class AppDbContext : DbContext //IdentityDbContext<ApplicationUser> //: DbContext
{
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }

    public AppDbContext() : base()
    {
    }
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlServer(ConnectionString.LoadConnectionString());
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

