// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using DotnetAPIBasicAuthentication.DTO;
// using DotnetAPIBasicAuthentication.Models;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.IdentityModel.Tokens;

// namespace DotnetAPIBasicAuthentication.Controllers;

// [ApiController]
// [Route("api/[controller]")]
// public class AccountController : ControllerBase
// {
//     private readonly UserManager<ApplicationUser> _userManager;
//     private readonly SignInManager<ApplicationUser> _signInManager;
//     private readonly IConfiguration _configuration;
//     public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
//     {
//         _userManager = userManager;
//         _signInManager = signInManager;
//         _configuration = configuration;
//     }
//     [HttpPost("register")]
//     public async Task<ActionResult<GenralResponse>> Register(RegisterDTO registerDTO)
//     {
//         if (ModelState.IsValid)
//         {
//             var user = new ApplicationUser
//             {
//                 UserName = registerDTO.UserName,
//                 Email = registerDTO.Email,
//                 Address = registerDTO.Address
//             };
//             var result = await _userManager.CreateAsync(user, registerDTO.Password);
//             if (result.Succeeded)
//             {
//                 return new GenralResponse()
//                 {
//                     Data = null,
//                     StatusCode = 200,
//                     IsSuccess = true,
//                     Message = "User registered successfully"
//                 };
//             }
//             else
//             {
//                 return new GenralResponse()
//                 {
//                     Data = null,
//                     StatusCode = 400,
//                     IsSuccess = false,
//                     Message = string.Join(",", result.Errors.Select(e => e.Description))
//                 };
//             }
//         }
//         else
//         {
//             return new GenralResponse()
//             {
//                 Data = null,
//                 StatusCode = 400,
//                 IsSuccess = false,
//                 Message = string.Join(",", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))
//             };
//         }
//     }

//     [HttpPost("login")]
//     public async Task<ActionResult<GenralResponse>> Login(LoginDTO LoginDTO)
//     {
//         //check if the model state is valid
//         if (ModelState.IsValid)
//         {
//             //check if the user exists
//             var user = await _userManager.FindByNameAsync(LoginDTO.UserName);
//             if (user != null)
//             {
//                 //check if the password is correct
//                 var found = await _userManager.CheckPasswordAsync(user, LoginDTO.Password);
//                 if (found)
//                 {
//                     //Generate a Claims
//                     var climes = new List<Claim>
//                     {
//                         new(ClaimTypes.NameIdentifier, user.Id),
//                         new (ClaimTypes.Email, user.Email ?? ""),
//                         new (ClaimTypes.Name, user.UserName ?? "")
//                     };
//                     var roles = await _userManager.GetRolesAsync(user);
//                     foreach (var role in roles)
//                     {
//                         climes.Add(new Claim(ClaimTypes.Role, role));
//                     }

//                     //Generate Token id clime(Jti) that will be changed each login
//                     climes.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

//                     //Generatesigning Credentials
//                     var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:key"] ?? ""));
//                     var signingCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

//                     //create the token
//                     var jwtSecurityToken = new JwtSecurityToken(
//                         //issuer: the server that create the token
//                         issuer: _configuration["jwt:issuer"],
//                         //audience: the server that will accept the token
//                         audience: _configuration["jwt:audience"],
//                         //claims: the claims that will be added to the token
//                         claims: climes,
//                         //expires: the expiration time of the token
//                         expires: DateTime.Now.AddHours(1),
//                         //signingCredentials: the credentials that will be used to sign the token
//                         signingCredentials: signingCredentials
//                     );

//                     return new GenralResponse()
//                     {
//                         Data = new
//                         {
//                             Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
//                             Expiration = jwtSecurityToken.ValidTo.ToString("yyyy-MM-ddTHH:mm:ssZ"),
//                             User = user
//                         },
//                         StatusCode = 200,
//                         IsSuccess = true,
//                         Message = "User logged in successfully"
//                     };
//                 }
//             }



//         }

//         return new GenralResponse()
//         {
//             Data = null,
//             StatusCode = 400,
//             IsSuccess = false,
//             Message = string.Join(",", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)))
//         };

//     }

// }
