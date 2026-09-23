//To make our code clean and maintainable we will use some concepts:
// ------------Seperation of Concerns (SOC)-----------
//is the process of separating the concerns of a software into distinct logical modules 
//in mvc the concerns are :
//  1- Model: represents the data and business logic
//  2- View: represents the user interface
//  3- Controller: represents the application logic that handles user input and interacts with the model and view
//  4- ViewModel: is a class that represents the data that is sent to the view
//  5- Repository Pattern: is a design pattern that is used to 
//      separate the data access logic from the application logic
//  6- Dependency Injection: is a design pattern that is used to inject the dependencies of a class into the class
//this help us to make our code more testable and maintainable and reusable
// and also help us to use DRY concept : Don't Repeat Yourself
//by moving the common code to the repository and using it in multiple controllers
//----------------------------------------------------

// ------------IOC (Inversion of Control)(Dependency Injection) -----------
//is a design pattern that is used to invert the control of the application
//in mvc the controller is responsible for creating the dependencies
//but in ioc the framework is responsible for creating the dependencies
//so the controller doesn't create the dependencies itself
//IOC Container (Service Provider):
//in c# the IOC container is the framework itself (program.cs file)
//
//we can do 3 operations on the services:
// 1- (Register) Add ==> we register the service in the container
//    this means we tell the container that we want to use this service
//    and we tell the container how to create it
//    AddSingleton() => create only one object for the service and share it accross the application 
//          builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
//    AddScoped() => create one object for the service and share it accross the request pipeline 
//          builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
//    AddTransient() => create new object for the service for each request 
//          builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
// 2- (Resolve) Create ==> we ask the container to create an instance of the service
//    when we use dependency injection, the container will create an instance of the service
//    and pass it to the constructor of the class that needs it
//    ex: public DepartmentController(IDepartmentRepository departmentRepository){}
//        here  IDepartmentRepository is an interface
//        and DepartmentRepository is an implementation of the interface
//        the controller doesn't know about the implementation of the interface
//        it only knows about the interface
//        this is called Dependency Injection
// 3- (Dispose) Remove ==> we tell the container to remove the service
//     this means we are done with the service
//     and the container will clean up the memory
//    the (Dispose()) method is called automatically when the object is no longer needed
//    or we can call it manually
//    ex: 
//    
// 

//we have 3 types of servises
// 1- Framwork Services:  already dclared and registered in the container
//2- built in servises :  already dclared but not registered in the container
//3- Custom Servises :  not dclared and not registered in the container





