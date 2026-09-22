using ServiceConsumer.Models;
using System.Net.Http.Json;

namespace ServiceConsumer;

public static class TestDotnetAPIBasics
{
    public static async Task Run()
    {
        HttpClient httpClient = new HttpClient();
        //httpClient.BaseAddress = new Uri("http://localhost:5260");
        //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await httpClient.GetFromJsonAsync<Department>("http://localhost:5260/api/Department/DTO/0");
        if (response is not null)
        {
            Console.WriteLine($"Name: {response.Name}, Manager: {response.ManagerName}, Employee Count: {response.EmpCount}");
        }
        else
        {
            Console.WriteLine("No response");
        }
    }
}


