
/*
ASP.NET MVC Identity Authentication Steps
1. Install Identity package:
    dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
2. Add class ApplicationUser.cs that inherits from IdentityUser:
    public class ApplicationUser : IdentityUser {......}
    we added it even we will not use it
3. Make AppDbContext inherit from IdentityDbContext<ApplicationUser> instead of DbContext:
    public class AppDbContext : IdentityDbContext<ApplicationUser> {......}
4. Add in the main function the identity services:
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
    
4. Add Migrations to update the database tables for identity:
    cd .\MVCIdentityAuthentication
    dotnet ef migrations add AddIdentity
    dotnet ef database update
5. Create AccountController.cs with a Register action:
    public class AccountController : Controller {.......}
6. Create RegisterViewModel.cs .
7. Create Register Action.
8. Register IdentityService in Program.cs:
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();


*/