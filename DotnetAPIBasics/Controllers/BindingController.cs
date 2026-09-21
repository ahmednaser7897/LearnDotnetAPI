using DotnetAPIBasics.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPIBasics.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BindController : ControllerBase
{
    // Data Binding - Binding data from the HTTP request to action parameters.
    // In API Controllers, ASP.NET Core can bind data from:
    //      1- Route Values
    //      2- Query String
    //      3- Request Body
    //      4- Headers
    // [ApiController] also enables automatic model validation and
    // automatically infers the binding source in many cases.
    //
    //==========================================================================================
    // 1- Primitive Type Parameters
    // Primitive parameters can be received from the Query String or Route Values.
    // Example using Query String:
    // http://localhost:5260/api/Bind/TestPrimitiveType?name=John&age=25
    // Output:
    // name: John, age:
    // If we send the same parameter more than once:
    // http://localhost:5260/api/Bind/TestPrimitiveType?name=John&name=Doe&age=25&age=30
    // For a single primitive parameter, model binding normally uses the first value.
    // Extra parameters that don't match an action parameter are ignored.
    // Example:
    // http://localhost:5260/api/Bind/TestPrimitiveType?name=John&age=25&weight=70
    // weight will be ignored.
    // If a parameter is not supplied, its default value is used.
    // Example:
    // http://localhost:5260/api/Bind/TestPrimitiveType?name=John
    // age will be 0 because int is a value type.
    //------------------------------------------------------------------------------------------
    // Route Values
    // We can receive a parameter from the URL path:
    // http://localhost:5260/api/Bind/TestPrimitiveType/1?name=John&age=25
    //
    // Here:
    //      id   => Route Value (Path Parameter)
    //      name => Query String
    //      age  => Query String
    //
    [HttpGet("TestPrimitiveType/{id}")]
    public IActionResult TestPrimitiveType(string name, int age, int id)
    {
        return Ok(new
        {
            name,
            age,
            id
        });
    }


    //==========================================================================================
    // 2- Array and Collection Parameters
    // We can bind multiple values to an array or collection.
    // 1- Repeated Query String Parameter:
    // http://localhost:5260/api/Bind/TestArrayParameter?names=John&names=Doe&ages=25&ages=30
    // Result:
    // names = [John, Doe]
    // ages  = [25, 30]
    //------------------------------------------------------------------------------------------
    // 2- Comma Separated Values:
    // http://localhost:5260/api/Bind/TestArrayParameter?names=John,Doe,Ahmed&ages=25,30,35
    //------------------------------------------------------------------------------------------
    // 3- Using Indexes:
    // http://localhost:5260/api/Bind/TestArrayParameter?names[0]=John&names[1]=Doe&ages[0]=25&ages[1]=30
    //
    [HttpGet("TestArrayParameter")]
    public IActionResult TestArrayParameter([FromBody] string[] names, [FromQuery] int[] ages)
    {
        return Ok(new
        {
            names,
            ages
        });
    }


    //==========================================================================================
    // 3- Dictionary Parameters
    // A Dictionary can be bound from Query String using the dictionary key syntax.
    // Example:
    // http://localhost:5260/api/Bind/TestDictionaryParameter?dict[key1]=1&dict[key2]=2
    // Result:
    // dict:
    //      key1 => 1
    //      key2 => 2
    [HttpGet("TestDictionaryParameter")]
    public IActionResult TestDictionaryParameter(
      [FromBody] Dictionary<string, int> dict)
    {
        return Ok(dict);
    }


    //==========================================================================================
    // 4- Complex Object Parameters
    // In API Controllers, a complex object parameter is normally bound from
    // the Request Body.
    // We can send JSON data in the request body.
    // Example:
    // POST http://localhost:5260/api/Bind/TestObjectParameter
    // Body:
    // {
    //     "id": 1,
    //     "name": "John",
    //     "managerName": "Doe"
    // }
    // [ApiController] automatically infers complex types from the body.
    // http://localhost:5260/api/Bind/TestObjectParameter/10?name=John
    // Here:
    //      id   => Route Value (Path Parameter)
    //      name => Query String
    //      department => Body
    [HttpPost("TestObjectParameter/{id}")]
    public IActionResult TestObjectParameter(Department department, string name, int id)
    {
        return Ok(new { name, department, id });
    }


    //==========================================================================================
    // 5- Explicit Binding Sources
    // We can explicitly tell ASP.NET Core where to get the parameter from.
    //------------------------------------------------------------------------------------------
    // [FromRoute]
    // Gets the value from the route.
    // Example:
    // http://localhost:5260/api/Bind/RouteExample/10
    //
    [HttpGet("RouteExample/{id}")]
    public IActionResult RouteExample([FromRoute] int id)
    {
        return Ok(id);
    }


    //------------------------------------------------------------------------------------------
    // [FromQuery]
    // Gets the value from the query string.
    // Example:
    // http://localhost:5260/api/Bind/QueryExample?name=John&age=25
    [HttpGet("QueryExample")]
    public IActionResult QueryExample(
        [FromQuery] string name,
        [FromQuery] int age)
    {
        return Ok(new
        {
            name,
            age
        });
    }


    //------------------------------------------------------------------------------------------
    // [FromBody]
    // Gets the value from the HTTP request body.
    // Usually used with JSON in POST, PUT or PATCH requests.
    // Example:
    // POST http://localhost:5260/api/Bind/BodyExample
    // Body:
    // {
    //     "id": 1,
    //     "name": "John"
    // }
    //
    [HttpPost("BodyExample")]
    public IActionResult BodyExample([FromBody] Department department)
    {
        return Ok(department);
    }


    //------------------------------------------------------------------------------------------
    // [FromHeader]
    // Gets the value from an HTTP header.
    // Example:
    // Authorization: Bearer token
    [HttpGet("HeaderExample")]
    public IActionResult HeaderExample(
        [FromHeader] string authorization)
    {
        return Ok(authorization);
    }


    //==========================================================================================
    // 6- Multiple Binding Sources
    // We can receive parameters from different places in the same action.
    // Example:
    // http://localhost:5260/api/Bind/MultipleSources/10?name=John
    // id   => Route
    // name => Query String
    // department => Body
    // Body:
    // {
    //     "id": 1,
    //     "name": "IT"
    // }
    //
    [HttpPost("MultipleSources/{id}")]
    public IActionResult MultipleSources(
        [FromRoute] int id,
        [FromQuery] string name,
        [FromBody] Department department)
    {
        return Ok(new
        {
            id,
            name,
            department
        });
    }


    //==========================================================================================
    // 7- Important Difference Between MVC Controller and API Controller
    // MVC Controller:
    //      public IActionResult Test(Department department)
    // Data can commonly be bound from Form Data, Route Values and Query String.
    // API Controller:
    //      [ApiController]
    // Complex types are automatically inferred from the Request Body.
    // Example:
    // POST /api/Bind/TestObjectParameter
    // Body:
    // {
    //     "id": 1,
    //     "name": "John"
    // }
    //
    //==========================================================================================
    // 8- [FromForm]
    // If we want to receive form data instead of JSON body, we can explicitly use [FromForm].
    // Example:
    // POST /api/Bind/FormExample
    // Content-Type: multipart/form-data
    [HttpPost("FormExample")]
    public IActionResult FormExample(
        [FromForm] string name,
        [FromForm] int age)
    {
        return Ok(new
        {
            name,
            age
        });
    }

}
