/*
   What is Configurations in ASP.NEW CORE?
   1- is a place where we can store settings for our application
   like
   - Database connections
   - API keys
   - Email settings
   - File paths
   - Any value that can change from one environment to another
   ------------------------------------------------
   The ASP.NET Has many prddifainde Sources to read main configurations from it
   like appsettings.json ,Environment Variables , user secrets,Command Line Arguments,Windows Registry
   The order of the sources is important
   The higher the source in the list, the higher its priority
   so if the same key is present in multiple sources, the last one will be used
   the default order of the sources is:
   1- appsettings.json
   2- appsettings.{Environment}.json
   3- appsettings.Production.json
   4- appsettings.Development.json
   5- Environment Variables
   6- User Secrets
   7- Command Line Arguments
   8- Windows Registry
   9- File Configuration
   ------------------------------------------------
   appsettings may has many versions like: 
   - appsettings.json (base)
   - appsettings.Development.json (development)
   - appsettings.Production.json (production)
   ------------------------------------------------
   appsettings.Development.json is the default version and used in development environment
   appsettings.Production.json is the default version and used in production environment
   appsettings.json is the base version and used in all environments
   ------------------------------------------------
   When we read from appsettings it will read from the base(appsettings.json) first
   then it will read from the environment version (appsettings.Development.json or appsettings.Production.json)
   and if it read a key from the environment version it will overwrite the value from the base version
   and if the key is not found in the environment version it will read from the base version
   ------------------------------------------------
   It will know the current environment version at the compile time not at the runtime
   from the (environmentVariables) section in launchSettings.json file (for dev mode )
   and for (production) mode it will know the current environment version at the build time
   from the build configuration (Debug or Release)
   the default value of the current environment version is Development
   we have many environment versions like:
   - Development
   - Production
   - Staging
   - Testing
   // Environment Variables is a varible in the operating system
   // we can read it from the environment
   // Environment Variables can ovveride appsettings.json file
   // like ASPNETCORE_ENVIRONMENT and DefaultConnection
   -----------------------------------------------------
   we can add our own configuration source like :
   -Configuration.json
    in program.cs file:
     builder.Configuration.AddJsonFile("Configuration.json");
    then we can read from it like this:
    _configuration["TestKey"];
    this file will override all the previous configuration sources
    --------------------------------------------------------
    We can add user secret for more security
    to add the keys that we don't want to save in the code
    like database connection strings or API keys
    after running the command: dotnet user-secrets init
    how to add user secret:
    1- open terminal
    2- navigate to the project directory
    3- run the command: dotnet user-secrets init
    4- run the command: dotnet user-secrets set "Key" "Value"
    ======================================================================
    we were using the normal way to read Configuration:
    Now we will use Options Pattern for cleaner code.
    first : define class for the configuration:
    AttachmentOptions.cs in Models folder
    then we have 3 ways to bind configuration to the class:
    1-GetSection().Get<T>()
        - we cand make it in the Program.cs file or in the controller file and inject the config.
            var attachmentOptions = builder.Configuration.GetSection("Attachment").Get<AttachmentOptions>();
            builder.Services.AddSingleton<AttachmentOptions>(attachmentOptions!);
        - in the controller
            var attachmentOptions = _configuration.GetSection("Attachment").Get<AttachmentOptions>();
            return Ok(attachmentOptions);
    2-Bind using GetSection() and GetValue()
        - we cand make it in the Program.cs file or in the controller file and inject the config.
            builder.Configuration.GetSection("Attachment").Bind(attachmentOptions);
            builder.Services.AddSingleton<AttachmentOptions>(attachmentOptions!);
        - in the controller
            var attachmentOptions = new AttachmentOptions();
            _configuration.GetSection("Attachment").Bind(attachmentOptions);
            return Ok(attachmentOptions);
    3-Bind using Options Pattern (Recommended)
        this way is more flexible because it allows us to change the configuration 
        without restarting the application
        and inject it in the controller using 
        IOptions<T> 
        => read once when the app starts and keep the value in memory(singleton)
        or IOptionSnapshot<T> 
        => read once for each request(scoped)
        => it reads config at first line it asked and save it in the memory for current request only 
        or IOptionMonitor<T>
        => read once for each request but with ability to refresh the configuration(singleton)
        => it listen to the changes in the configuration files and update the value automatically
        => it calls the GetValue() method on every method call (not only first time)
        - we cand make it in the Program.cs file or in the controller file and inject the config.
            builder.Services.Configure<AttachmentOptions>(builder.Configuration.GetSection("Attachment"));;
        - in the controller
            public ConfigurationController(IOptions<AttachmentOptions> attachmentOptions)
            {
                _attachmentOptions = attachmentOptions.Value;
            }
            return Ok(attachmentOptions);
*/
