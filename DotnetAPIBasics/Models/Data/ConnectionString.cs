using Microsoft.Extensions.Configuration;
namespace DotnetAPIBasics.Models.Data;

internal static class ConnectionString
{
    public static string LoadConnectionString()
    {
        var configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
        var connectionString = configuration.GetSection("connectStrings").Value;
        return connectionString ?? "";
    }
}

