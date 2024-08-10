using Backend.Application;
using Backend.Infrastructure;
using Backend.Infrastructure.Extensions;
using Backend.Web;
using Backend.Web.Infrastructure;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddApplicationServices();

    var databaseConnection = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddInfrastructureServices(databaseConnection);
    builder.Services.AddWebServices(builder.Configuration);
    builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

    var app = builder.Build();

    await app.InitialiseAsync();
    if (!app.Environment.IsDevelopment())
    {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
        app.UseHttpsRedirection();
    }

    app.UseHealthChecks("/health");
    app.UseStaticFiles();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseExceptionHandler(options => { });

    app.UseCors();

    app.MapEndpoints();
    app.MapGet("/", () => "Web API v1.0");

    app.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
    return 1;
}
finally
{
    await Log.CloseAndFlushAsync();
}

namespace Backend.Web
{
    public class Program;
}
