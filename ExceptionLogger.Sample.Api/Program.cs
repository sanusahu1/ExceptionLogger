using ExceptionLogger.Extensions;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add ExceptionLogger service
builder.Services.AddExceptionLogging(options =>
{
    options.IncludeStackTrace = true;
    options.IncludeInnerExceptions = true;
    options.IncludeSystemInfo = true;
    options.IncludeRequestInfo = true;
    //options.MaxInnerExceptionDepth = 5;
    options.PathToStore = "C:/Users/SmrutiRanjanSahu/Desktop/NugetPkg/ExceptionLogger/ExceptionLogger.Sample.Api/Exceptions/";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add ExceptionLogger middleware
app.UseExceptionLogging(
    applicationName: "ExceptionLogger.Sample.Api",
    environment: app.Environment.EnvironmentName
);

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();