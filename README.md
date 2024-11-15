# ExceptionLogger
EasyException
EasyException is a powerful and easy-to-use NuGet package for handling and logging exceptions in .NET applications. It provides centralized exception handling, logging, and context capture, with additional features like local exception storage and automated cleanup.

Features
Centralized exception logging for ASP.NET Core applications.
Automatic enrichment of logs with:
Stack trace
Inner exceptions
System information (OS, runtime)
Request details (headers, path, query)
Flexible configuration options for customization.
Local exception storage for debugging purposes.
Automatic cleanup of stored exceptions after 24 hours.
Seamless integration with popular logging frameworks like Serilog, NLog, or Microsoft.Extensions.Logging.
Getting Started
Installation
Add the package to your project via NuGet Package Manager or the .NET CLI:

bash
Copy code
dotnet add package EasyException
Configuration
Add EasyException to your service container in the Startup.cs or Program.cs file:

csharp
Copy code
builder.Services.AddExceptionLogging(options =>
{
    options.IncludeStackTrace = true;           // Include stack trace in logs
    options.IncludeInnerExceptions = true;     // Include details of inner exceptions
    options.IncludeSystemInfo = true;          // Log system details (OS, runtime info)
    options.IncludeRequestInfo = true;         // Log request metadata (path, query, headers)
    
    // Maximum depth of inner exceptions to log (optional)
    // options.MaxInnerExceptionDepth = 5;
    
    // Path to store logged exceptions locally (for debugging purposes)
    options.PathToStore = "C:/Path/To/Exceptions/"; 
});
Middleware Usage
Add the exception logging middleware to your application's request pipeline:

csharp
Copy code
app.UseExceptionLogging();
This middleware will capture and log all unhandled exceptions, enrich them with additional context, and optionally store them locally.

Exception Storage
Local Storage
Exceptions are stored locally for debugging. To enable this feature, set the PathToStore property in the configuration.
Example path: C:/Path/To/Exceptions/.

Automatic Cleanup
Logged exceptions are automatically deleted after 24 hours to avoid unnecessary clutter.

Enriching Logs
By default, EasyException enriches logs with the following details:

Stack Trace: Helps trace the origin of the error.
Inner Exceptions: Provides details about nested exceptions.
System Info: Logs runtime details, including OS and environment.
Request Info: Captures HTTP request metadata (headers, query string, etc.).
All these enrichments are configurable through the AddExceptionLogging options.

Usage Example
Here's an example of how to configure and use the package in an ASP.NET Core application:

Configuration
csharp
Copy code
builder.Services.AddExceptionLogging(options =>
{
    options.IncludeStackTrace = true;
    options.IncludeInnerExceptions = true;
    options.IncludeSystemInfo = true;
    options.IncludeRequestInfo = true;
    options.PathToStore = "C:/Exceptions/";
});
Middleware Setup
csharp
Copy code
app.UseExceptionLogging();
Custom Logging
You can also manually log exceptions with the included services:

csharp
Copy code
var exceptionLogger = app.Services.GetRequiredService<IExceptionLogger>();

try
{
    // Your application logic
}
catch (Exception ex)
{
    exceptionLogger.LogException(ex);
}
NuGet Package Details
Package ID: EasyException
Version: 1.0.0
License: MIT
Platform Support: .NET 6+, .NET Core 3.1+
Roadmap
Support for distributed logging (e.g., Azure Blob Storage, AWS S3).
Integration with monitoring tools like Application Insights and Prometheus.
Enhanced UI for local exception visualization.
Contributing
Contributions are welcome! Please submit pull requests or open issues for bugs or feature requests.

License
This project is licensed under the MIT License. See the LICENSE file for details.

Contact
For support or inquiries, contact [sanusahu255@gmail.com].

Enjoy using EasyException to simplify your exception handling and logging! 🚀
