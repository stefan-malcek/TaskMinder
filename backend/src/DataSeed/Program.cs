using Backend.Application;
using Backend.Application.Common.Options;
using Backend.DataSeed.Extensions;
using Backend.Infrastructure;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

Console.WriteLine("Starting the seeding process.\n");
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddApplicationServices();

    var currentDirectory = Directory.GetCurrentDirectory();
    Console.WriteLine($"Current dir: {currentDirectory}\n");

    var rootProjectIndex = currentDirectory.IndexOf("DataSeed");
    var rootProjectPath = currentDirectory[..rootProjectIndex];
    var basePath = Path.Combine(rootProjectPath, "Web");
    var configuration = builder.Configuration
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json")
        .AddJsonFile("appsettings.Development.json")
        .AddEnvironmentVariables()
        .Build();

    var databaseConnection = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddInfrastructureServices(databaseConnection);

    var services = builder.Services;
    var applicationDbContext = builder.Services.SingleOrDefault(d => d.ServiceType == typeof(ApplicationDbContext));
    if (applicationDbContext != null)
    {
        services.Remove(applicationDbContext);
    }

    services.AddScoped<ApplicationDbContext>(_ =>
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (!connectionString!.ToLower().Contains("127.0.0.1"))
        {
            throw new Exception("You can drop and recreate a database only on localhost!");
        }

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(connectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);

        return new ApplicationDbContext(optionsBuilder.Options,
            Options.Create(new DbContextSettings { EnableSensitiveLogging = true }));
    });

    var databaseInitializer = services.SingleOrDefault(d => d.ServiceType == typeof(ApplicationDbContextInitializer));
    if (databaseInitializer != null)
    {
        services.Remove(databaseInitializer);
    }

    var app = builder.Build();
    var dbContext = app.Services.GetRequiredService<ApplicationDbContext>();

    Console.WriteLine("Deleting the previous database.");
    dbContext.Database.EnsureDeleted();

    Console.WriteLine("Running migrations.");
    dbContext.Database.Migrate();

    Console.WriteLine("Seeding users.");
    var userManager = app.Services.GetRequiredService<UserManager<ApplicationUser>>();
    await userManager.SeedUsersAsync();

    Console.WriteLine("Seeding example data.");
    await dbContext.SeedEntitiesAsync();

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("\nSeeding was successful.");
}
catch (Exception e)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("\nSeeding failed.");

    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine(e.Message);
    Console.WriteLine(e.StackTrace);
    throw;
}
finally
{
    Console.ForegroundColor = ConsoleColor.White;
}
