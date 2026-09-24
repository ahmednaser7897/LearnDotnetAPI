using DotnetAPIBasics.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace DotnetAPIBasics.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConfigurationController(IConfiguration configuration, IOptions<AttachmentOptions> attachmentOptions) : ControllerBase
{
    //to read app configuration  we must have opject of type IConfiguration
    //which is built in service and registered in the container 
    //so we can read it from the constructor
    private readonly IConfiguration _configuration = configuration;
    private readonly IOptions<AttachmentOptions> _attachmentOptions = attachmentOptions;


    [HttpGet("IConfiguration")]
    public IActionResult GetConfigurations()
    {

        return Ok(
            new
            {
                //from appsettings.json file and its versions
                ConnectionStringFromGetConnectionString = _configuration.GetConnectionString("DefaultConnection"),
                ConnectionStringFromNested = _configuration["ConnectionStrings:DefaultConnection"],
                AllowedHosts = _configuration["AllowedHosts"],
                Logging = new
                {
                    LogLevel = new
                    {
                        Default = _configuration["Logging:LogLevel:Default"],
                        MicrosoftAspNetCore = _configuration["Logging:LogLevel:Microsoft.AspNetCore"]
                    },
                },

                //-----------------------------------------------------------------------------------
                //from (environmentVariables) section in launchSettings.json file
                //environmentVariables can ovveride appsettings.json file
                ASPNETCORE_ENVIRONMENT = _configuration["ASPNETCORE_ENVIRONMENT"],

                //-----------------------------------------------------------------------------------
                //from (Configuration.json) file
                TestKey = _configuration["TestKey"],

                //-----------------------------------------------------------------------------------
                //from user secrets file
                SigningKey = _configuration["SigningKey"],
            }
        );
    }

    [HttpGet("GetSectionGet")]
    public IActionResult GetSectionGetConfigurations()
    {
        var attachmentOptions = _configuration.GetSection("Attachment").Get<AttachmentOptions>();

        return Ok(
            new
            {
                //from appsettings.json file and its versions
                ConnectionStringFromGetConnectionString = _configuration.GetConnectionString("DefaultConnection"),
                ConnectionStringFromNested = _configuration["ConnectionStrings:DefaultConnection"],
                AllowedHosts = _configuration["AllowedHosts"],
                Logging = new
                {
                    LogLevel = new
                    {
                        Default = _configuration["Logging:LogLevel:Default"],
                        MicrosoftAspNetCore = _configuration["Logging:LogLevel:Microsoft.AspNetCore"]
                    },
                },
                //-----------------------------------------------------------------------------------
                //from Options Pattern
                AttachmentOptions = attachmentOptions,
            }
        );
    }

    [HttpGet("GetSectionBind")]
    public IActionResult GetSectionBindConfigurations()
    {
        AttachmentOptions attachmentOptions = new();
        _configuration.GetSection("Attachment").Bind(attachmentOptions);

        return Ok(
            new
            {
                //from appsettings.json file and its versions
                ConnectionStringFromGetConnectionString = _configuration.GetConnectionString("DefaultConnection"),
                ConnectionStringFromNested = _configuration["ConnectionStrings:DefaultConnection"],
                AllowedHosts = _configuration["AllowedHosts"],
                Logging = new
                {
                    LogLevel = new
                    {
                        Default = _configuration["Logging:LogLevel:Default"],
                        MicrosoftAspNetCore = _configuration["Logging:LogLevel:Microsoft.AspNetCore"]
                    },
                },
                //-----------------------------------------------------------------------------------
                //from Options Pattern
                AttachmentOptions = attachmentOptions,
            }
        );
    }

    [HttpGet("ServicesConfigure")]
    public IActionResult GetServicesConfigureOptions()
    {


        return Ok(
            new
            {
                //from appsettings.json file and its versions
                ConnectionStringFromGetConnectionString = _configuration.GetConnectionString("DefaultConnection"),
                ConnectionStringFromNested = _configuration["ConnectionStrings:DefaultConnection"],
                AllowedHosts = _configuration["AllowedHosts"],
                Logging = new
                {
                    LogLevel = new
                    {
                        Default = _configuration["Logging:LogLevel:Default"],
                        MicrosoftAspNetCore = _configuration["Logging:LogLevel:Microsoft.AspNetCore"]
                    },
                },
                //-----------------------------------------------------------------------------------
                //from Options Pattern
                AttachmentOptions = _attachmentOptions.Value,
            }
        );
    }

}
