

using static System.Security.Claims.ClaimTypes;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPIBasicAuthentication.Authentication;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory loggerFactory,
        UrlEncoder encoder) :
        base(options, loggerFactory, encoder)
    { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        //here we write the logic of how to authenticate the user
        //like checking username and password
        //using IUserService interface
        //if success return AuthenticateResult.Success(ticket);
        //if failed return AuthenticateResult.Fail("Authentication failed");
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }
        String? authHeader = Request.Headers["Authorization"].ToString();
        if (authHeader.IsNullOrEmpty() || !authHeader.StartsWith("Basic", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(AuthenticateResult.Fail("Schema is not Basic"));
        }
        String encodedSchemeAndCredentials = authHeader["Basic ".Length..];
        String encodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedSchemeAndCredentials));
        String[] credentials = encodedCredentials.Split(':');
        String username = credentials[0];
        String password = credentials[1];

        if (username == "admin" && password == "password")
        {
            var claims = new List<Claim>()
            {
                new (ClaimTypes.NameIdentifier, "1"),
                new (ClaimTypes.Name, username),
                new (ClaimTypes.Role, "admin")
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        return Task.FromResult(AuthenticateResult.Fail("Authentication failed"));
    }
}
