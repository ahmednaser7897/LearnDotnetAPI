using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace DotnetAPIBasics.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class HandelErorrAttribute : Attribute, IExceptionFilter // ==>IExceptionFilter inhert IFilterMetadata
{   //This filter will execute when an exception is thrown in the controller or action
    //it gives us all data we want to know about this request : the exception type,stack trace,request data,ect
    public void OnException(ExceptionContext context)
    {
        context.Result = new NotFoundResult();
    }
}
