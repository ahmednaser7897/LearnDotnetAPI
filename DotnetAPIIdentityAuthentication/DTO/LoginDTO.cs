using System.ComponentModel.DataAnnotations;

namespace DotnetAPIIdentityAuthentication.DTO;

public class LoginDTO
{
    [Required]
    public required string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

}

