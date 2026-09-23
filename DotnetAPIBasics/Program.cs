using DotnetAPIBasics.Models;
using DotnetAPIBasics.Models.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
namespace DotnetAPIBasics;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        //builder.Services.AddControllers();
        // Disable the automatic validation of the model state 
        // which returns 400 Bad Request
        // and enable the custom validation.
        builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            options.SuppressModelStateInvalidFilter = true
        );
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
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

        //add DbContext and repositories to the container
        builder.Services.AddDbContext<AppDbContext>(
            options => options.UseSqlServer(ConnectionString.LoadConnectionString())
        );
        //------------------------------------------------------------------
        //For Authentication
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
        options =>
        {
            //options.Password
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = true;
        }
        ).AddEntityFrameworkStores<AppDbContext>();
        //for JWT Authentication 
        //This configuration is applied when the application starts and is used by the application to authenticate requests.
        builder.Services.AddAuthentication(
          options =>
          {
              //This is the default authentication scheme that will be used to authenticate requests.
              options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
              //This is the default challenge scheme that will be used to challenge requests.
              options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
              //This is the default scheme that will be used to authenticate requests.
              options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
          }
          ).AddJwtBearer(
              options =>
              {
                  //this option is used to save the token in the client side
                  options.SaveToken = true;
                  //this option is used to require the https metadata
                  options.RequireHttpsMetadata = false;
                  //this option is used to validate the token
                  options.TokenValidationParameters =
                  new TokenValidationParameters
                  {
                      //this option is used to validate the issuer
                      ValidateIssuer = true,
                      //this option is used to validate the audience
                      ValidateAudience = true,
                      //this option is used to validate the lifetime
                      ValidateLifetime = true,
                      //this option is used to validate the issuer signing key
                      ValidateIssuerSigningKey = true,
                      //this option is used to set the issuer
                      ValidIssuer = builder.Configuration["jwt:issuer"],
                      //this option is used to set the audience
                      ValidAudience = builder.Configuration["jwt:audience"],
                      //this option is used to set the issuer signing key
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"] ?? ""))

                  };

              }
          );
        //------------------------------------------------------------------
        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

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

        var app = builder.Build();

        app.UseStaticFiles();
        app.UseCors("AllowAll");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}

