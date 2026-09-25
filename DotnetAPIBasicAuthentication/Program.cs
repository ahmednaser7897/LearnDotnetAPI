using DotnetAPIBasicAuthentication.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
namespace DotnetAPIBasicAuthentication;

public static class Program
{
    public static void Main(string[] args)
    {
        //------------------------------------------------
        //Create Builder
        var builder = WebApplication.CreateBuilder(args);
        //------------------------------------------------
        //Set up logging
        // builder.Services.AddLogging(logger =>
        // {
        //     //logger.AddConsole();
        //     logger.AddDebug();

        // });
        //------------------------------------------------
        //Configuration(builder);
        //------------------------------------------------
        // Add services to the container (Inversion of Control).
        //see: DotnetAPIBasicAuthentication/DependencyInjection.cs
        // we have 3 types of servises
        // 1- Framwork Services: already dclared and registered in the container:ex:ILogger,IConfiguration;
        // 2- Built in servises : already dclared but not registered in the container :ex:AddDbContext,AddIdentity,AddJWTAuthentication,AddCors;
        // 3- Custom Servises :  not dclared and not registered in the container :ex:IEmployeeRepository,IDepartmentRepository;

        //Built in servises
        AddControllers(builder);
        //Built in servises
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();

        //Built in servises
        AddSwaggerGen(builder);
        AddDbContext(builder);

        //Custom servises
        AddCustomServises(builder);

        //Built in servises
        //AddIdentity(builder);
        //AddJWTAuthentication(builder);
        AddCors(builder);

        //------------------------------------------------
        // Building the Application 
        var app = builder.Build();

        //------------------------------------------------
        //Configuring the HTTP request pipeline (Middleware)


        app.UseStaticFiles();
        app.UseCors("AllowAll");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        // app.UseMiddleware<ProfilingMiddleware>();
        // app.UseMiddleware<RateLimitingMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }

    // private static void Configuration(WebApplicationBuilder builder)
    // {
    //     //The ASP.NET Has many prddifainde Sources to read main configurations from it
    //     //like appsettings.json ,Environment Variables , user secrets,Command Line Arguments,Windows Registry
    //     //we can add our own configuration source like :
    //     //-Configuration.json
    //     //in program.cs file:
    //     //builder.Configuration.AddJsonFile("Configuration.json");
    //     //then we can read from it like this:
    //     //_configuration["TestKey"];
    //     //this file will override all the previous configuration sources
    //     //builder.Configuration.AddJsonFile("Configuration.json");
    //     //----------------------------------------------------
    //     //read Attachment section in the appsettings.json file and bind it to AttachmentOptions class
    //     //var attachmentOptions = builder.Configuration.GetSection("Attachment").Get<AttachmentOptions>();
    //     //Inject AttachmentOptions class in the container
    //     //builder.Services.AddSingleton<AttachmentOptions>(attachmentOptions!);
    //     //this is th recommended way to bind configuration to the class
    //     //it add the class to the container and we can inject it in the constructor using 
    //     //1- IOptions<AttachmentOptions>
    //     //2- IOptionSnapshot<AttachmentOptions>
    //     //3- IOptionMonitor<AttachmentOptions>
    //     //builder.Services.Configure<AttachmentOptions>(builder.Configuration.GetSection("Attachment"));
    // }

    private static void AddControllers(WebApplicationBuilder builder)
    {
        //builder.Services.AddControllers();
        // Disable the automatic validation of the model state 
        // which returns 400 Bad Request
        // and enable the custom validation.
        builder.Services.AddControllers(
        // apply filter to all controllers
        //  options =>
        //  {
        //      // options.Filters.Add<LogActivityFilterAttribute>();
        //      // options.Filters.Add<HandelErorrAttribute>();
        //  }
        ).ConfigureApiBehaviorOptions(options =>
            options.SuppressModelStateInvalidFilter = true
        );
    }

    private static void AddSwaggerGen(WebApplicationBuilder builder)
    {
        // we a
        //builder.Services.AddSwaggerGen();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token"
            });

            options.AddSecurityRequirement(document =>
                new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] =
                        []
                });
        });
    }

    private static void AddDbContext(WebApplicationBuilder builder)
    {
        //for entity framework (DbContext)
        //add DbContext to the container
        builder.Services.AddDbContext<AppDbContext>(
            options => options.UseSqlServer(ConnectionString.LoadConnectionString())
        );
    }

    private static void AddCustomServises(WebApplicationBuilder builder)
    {
        //add custom services to the container
        //Custom Servises : not dclared and not registered in the container
        //AddSingleton() => create only one object for the service and share it accross the application 
        //builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
        //AddScoped() => create one object for the service and share it accross the request pipeline 
        //builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        //AddTransient() => create new object for the service for each request 
        //for Repositories
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
    }

    // private static void AddIdentity(WebApplicationBuilder builder)
    // {
    //     //For Authentication
    //     builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
    //     options =>
    //     {
    //         //options.Password
    //         options.Password.RequireDigit = true;
    //         options.Password.RequireLowercase = true;
    //         options.Password.RequireUppercase = true;
    //         options.Password.RequiredLength = 6;
    //         options.Password.RequireNonAlphanumeric = true;
    //     }
    //     ).AddEntityFrameworkStores<AppDbContext>();
    // }

    // private static void AddJWTAuthentication(WebApplicationBuilder builder)
    // {

    //     //for JWT Authentication 
    //     //This configuration is applied when the application starts and is used by the application to authenticate requests.
    //     builder.Services.AddAuthentication(
    //       options =>
    //       {
    //           //This is the default authentication scheme that will be used to authenticate requests.
    //           options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //           //This is the default challenge scheme that will be used to challenge requests.
    //           options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //           //This is the default scheme that will be used to authenticate requests.
    //           options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    //       }
    //       ).AddJwtBearer(
    //           options =>
    //           {
    //               //this option is used to save the token in the client side
    //               options.SaveToken = true;
    //               //this option is used to require the https metadata
    //               options.RequireHttpsMetadata = false;
    //               //this option is used to validate the token
    //               options.TokenValidationParameters =
    //               new TokenValidationParameters
    //               {
    //                   //this option is used to validate the issuer
    //                   ValidateIssuer = true,
    //                   //this option is used to validate the audience
    //                   ValidateAudience = true,
    //                   //this option is used to validate the lifetime
    //                   ValidateLifetime = true,
    //                   //this option is used to validate the issuer signing key
    //                   ValidateIssuerSigningKey = true,
    //                   //this option is used to set the issuer
    //                   ValidIssuer = builder.Configuration["jwt:issuer"],
    //                   //this option is used to set the audience
    //                   ValidAudience = builder.Configuration["jwt:audience"],
    //                   //this option is used to set the issuer signing key
    //                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"] ?? ""))

    //               };

    //           }
    //       );
    // }

    private static void AddCors(WebApplicationBuilder builder)
    {
        // this line add the cors policy to the container and this enable cross-origin requests
        // from the frontend (Angular in this case) to the backend (ASP.NET Core API).
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyMethod();
                policy.AllowAnyHeader();
            });
        });
    }

}

