using DotnetAPIBasicAuthentication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetAPIBasicAuthentication.Models.Data.Config;


public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn(0, 1);

        builder.Property(x => x.Name)
            .HasColumnType("VARCHAR")
            .HasMaxLength(255).IsRequired();

        builder.Property(x => x.ManagerName)
           .HasColumnType("VARCHAR")
           .HasMaxLength(255).IsRequired();

        builder.ToTable("Departments");
    }
}
