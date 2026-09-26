/*
    What is Authentication in ASP.NET Core ?
    Authentication is the process of verifying the identity of a user.

    There are two types of authentication in api apps:
    1. Basic Authentication : it used for web application
    2. Token Based Authentication : it used for web API
    ------------------------------------------------------------------
    What is  Basic Authentication in ASP.NET Core?
    its a way of Authentication that check if the user valid in each request.
    by sending username and password in the header of the request sprted by : and converted to base64 and append "Basic " to it.
    Example of the header:
    Authorization: Basic base64(username:password)
        ex Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==
    ------------------------------------------------------------------
    Steps of Basic Authentication:
        1. In Program.cs 
            -> add authentication scheme 
                builder.Services.AddAuthentication().
                AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);
            -> app.UseAuthentication();
            -> app.UseAuthorization();
        2. In Controller
            -> add [Authorize] attribute to the controller or actions
            -> add [AllowAnonymous] attribute to the actions that you want to allow anonymous access to it
        3. Create class BasicAuthenticationHandler that handles the authentication
            -> inherit from AuthenticationHandler<AuthenticationSchemeOptions>
            -> override HandleAuthenticateAsync method
            public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
        4. to get the logged in user in the controller
            -> var userIdentity = User.Identity?.Name;
            -> var role = User.FindFirst(ClaimTypes.Role)?.Value;
            -> var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            -> var claims = User.Claims;
            -> Note : if the user is not authenticated, the User.Identity will be null
            -> if the user is not authenticated, the User.FindFirst will return null
            -> if the user is not authenticated, the User.Claims will return null  
*/
