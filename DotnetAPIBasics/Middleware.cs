/*
# HTTP Request, HTTP Pipeline, Middleware, IIS & Proxy Server in .NET

## 1. What happens when a user sends a request?

When a client (browser, mobile app, Postman, etc.) wants to communicate with a .NET application, it sends an **HTTP Request**.

Example:

```http
GET https://example.com/Employee/Details/5
```

The request contains information such as:

* HTTP Method → `GET`, `POST`, `PUT`, `DELETE`
* URL
* Headers
* Query parameters
* Body (mainly with POST/PUT/PATCH)
* Cookies

The request then travels through several layers before reaching our application.

---

# 2. IIS

**IIS (Internet Information Services)** is Microsoft's web server for Windows.

When an ASP.NET Core application is deployed on Windows, IIS can receive the incoming HTTP request before forwarding it to the ASP.NET Core application.

Simplified flow:

```text
Client
   ↓
IIS
   ↓
ASP.NET Core Application
   ↓
HTTP Pipeline
   ↓
Middleware
   ↓
Controller / Endpoint
```

IIS can handle things such as:

* Receiving HTTP/HTTPS requests
* Hosting websites
* SSL/HTTPS configuration
* Process management
* Logging
* Forwarding requests to ASP.NET Core

For ASP.NET Core, IIS commonly works as a **reverse proxy** in front of the application.

---

# 3. Proxy Server

A **Proxy Server** is a server that sits between the client and another server.

```text
Client
   ↓
Proxy Server
   ↓
Web Server
   ↓
Application
```

The client does not communicate directly with the final server.

### Reverse Proxy

With a **Reverse Proxy**, the proxy sits in front of the application/server.

```text
Client
   ↓
Reverse Proxy
   ↓
ASP.NET Core
```

Examples of reverse proxies include:

* IIS
* Nginx
* Apache
* Cloud/load-balancing services

A reverse proxy can provide:

* HTTPS termination
* Routing
* Load balancing
* Security
* Request forwarding
* Multiple applications behind one public domain

---

# 4. ASP.NET Core HTTP Pipeline

After the request reaches the ASP.NET Core application, it enters the **HTTP Pipeline**.

The HTTP Pipeline is the sequence of components that process the request and response.

```text
HTTP Request
     ↓
ASP.NET Core Pipeline
     ↓
Middleware 1
     ↓
Middleware 2
     ↓
Middleware 3
     ↓
Routing
     ↓
Authentication
     ↓
Authorization
     ↓
Controller / Endpoint
     ↓
HTTP Response
```

The request moves through the pipeline in the order in which the middleware was registered.

---

# 5. Middleware

A **Middleware** is a component in the ASP.NET Core HTTP Pipeline.

Middleware can:

* Inspect the request
* Modify the request
* Perform some operation
* Call the next middleware
* Modify the response
* Stop the request and return a response

Example:

```csharp
app.Use(async (context, next) =>
{
    Console.WriteLine("Before");

    await next();

    Console.WriteLine("After");
});
```

`next()` means:

> Continue processing the request with the next middleware.

---

# 6. Middleware Flow

Suppose we have three middleware components:

```csharp
app.Use(async (context, next) =>
{
    Console.WriteLine("Middleware 1 Before");

    await next();

    Console.WriteLine("Middleware 1 After");
});

app.Use(async (context, next) =>
{
    Console.WriteLine("Middleware 2 Before");

    await next();

    Console.WriteLine("Middleware 2 After");
});

app.Use(async (context, next) =>
{
    Console.WriteLine("Middleware 3 Before");

    await next();

    Console.WriteLine("Middleware 3 After");
});
```

The execution will look like:

```text
Request
   ↓
Middleware 1 Before
   ↓
Middleware 2 Before
   ↓
Middleware 3 Before
   ↓
Controller / Endpoint
   ↓
Middleware 3 After
   ↓
Middleware 2 After
   ↓
Middleware 1 After
   ↓
Response
```

This is why middleware is sometimes described as a **pipeline or chain**.

---

# 7. Middleware can stop the Pipeline

Middleware does not have to call `next()`.

Example:

```csharp
app.Use(async (context, next) =>
{
    if (!context.Request.Headers.ContainsKey("Authorization"))
    {
        context.Response.StatusCode = 401;
        return;
    }

    await next();
});
```

If the request doesn't contain the required header:

```text
Request
   ↓
Middleware
   ↓
Authorization Header?
   ↓
   NO
   ↓
401 Unauthorized
```

The request never reaches the next middleware or controller.

---

# 8. Common ASP.NET Core Middleware

ASP.NET Core provides many middleware components.

For example:

```csharp
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
```

Some important ones are:

### UseHttpsRedirection()

Redirects HTTP requests to HTTPS.

```text
HTTP
 ↓
HTTPS
```

### UseStaticFiles()

Allows the application to serve static files such as:

```text
CSS
JavaScript
Images
```

### UseRouting()

Determines which endpoint should handle the request.

For example:

```text
/Employee/Details/5
        ↓
EmployeeController
        ↓
Details(5)
```

### UseAuthentication()

Determines **who the user is**.

```text
Request
 ↓
Authentication
 ↓
User Identity
```

### UseAuthorization()

Determines whether the authenticated user is **allowed** to access a resource.

```text
Who are you?
     ↓
Authentication

Are you allowed?
     ↓
Authorization
```

---

# 9. Complete Request Flow

Putting everything together:

```text
                CLIENT
                  │
                  │ HTTP Request
                  ▼
          ┌─────────────────┐
          │  Proxy / IIS    │
          │  Reverse Proxy  │
          └────────┬────────┘
                   │
                   │ Forward Request
                   ▼
          ┌─────────────────┐
          │ ASP.NET Core    │
          │ HTTP Pipeline   │
          └────────┬────────┘
                   │
                   ▼
          ┌─────────────────┐
          │ Middleware 1    │
          └────────┬────────┘
                   ▼
          ┌─────────────────┐
          │ Middleware 2    │
          └────────┬────────┘
                   ▼
          ┌─────────────────┐
          │ Routing         │
          └────────┬────────┘
                   ▼
          ┌─────────────────┐
          │ Authentication  │
          └────────┬────────┘
                   ▼
          ┌─────────────────┐
          │ Authorization   │
          └────────┬────────┘
                   ▼
          ┌─────────────────┐
          │ Controller       │
          │ / Endpoint       │
          └────────┬────────┘
                   │
                   ▼
                RESPONSE
                   │
                   ▼
          IIS / Reverse Proxy
                   │
                   ▼
                CLIENT
```

---

# 10. Important Difference

Don't confuse these concepts:

| Concept           | Meaning                                                     |
| ----------------- | ----------------------------------------------------------- |
| **HTTP Request**  | Message sent by the client to the server                    |
| **IIS**           | Microsoft web server that can receive and forward requests  |
| **Proxy**         | Server that sits between client and destination             |
| **Reverse Proxy** | Proxy that sits in front of backend servers                 |
| **HTTP Pipeline** | Sequence through which a request passes inside ASP.NET Core |
| **Middleware**    | Individual component inside the ASP.NET Core pipeline       |
| **Routing**       | Determines which endpoint should handle the request         |
| **Controller**    | Handles application requests/actions in MVC/API             |

---

# 11. Simple Real-World Example

Suppose the browser requests:

```http
GET https://mywebsite.com/Employee/Details/5
```

The process can be understood as:

```text
1. Browser creates HTTP Request
              ↓
2. Request reaches IIS / Reverse Proxy
              ↓
3. IIS forwards request to ASP.NET Core
              ↓
4. Request enters HTTP Pipeline
              ↓
5. Middleware processes the request
              ↓
6. Routing finds EmployeeController
              ↓
7. Authentication checks the user
              ↓
8. Authorization checks permissions
              ↓
9. Controller Action executes
              ↓
10. Controller returns Response
              ↓
11. Response travels back through Pipeline
              ↓
12. IIS / Proxy sends Response to Browser
```

### The main idea to remember

```text
CLIENT
  ↓
HTTP REQUEST
  ↓
IIS / REVERSE PROXY
  ↓
ASP.NET CORE
  ↓
HTTP PIPELINE
  ↓
MIDDLEWARES
  ↓
ROUTING
  ↓
CONTROLLER / ENDPOINT
  ↓
HTTP RESPONSE
  ↓
CLIENT
```

**Pipeline = the road**

**Middleware = checkpoints on the road**

**IIS / Reverse Proxy = the server/gateway that receives the request before it reaches the application**

**Controller/Endpoint = the destination that finally handles the request**

*/
