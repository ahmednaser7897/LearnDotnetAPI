using Microsoft.AspNetCore.Identity;

namespace DotnetAPIBasics.Models;
//We add this model so we can add more fields to the default user model provided by 
// ASP.NET Identity framework eg. Age, Gender, 
// The ApplicationUser model inherits from IdentityUser class which 
// gives us all the features of the default user model like username, email, password, etc.
// In this example we added Address field
public class ApplicationUser : IdentityUser
{
    public string? Address { get; set; }
}
