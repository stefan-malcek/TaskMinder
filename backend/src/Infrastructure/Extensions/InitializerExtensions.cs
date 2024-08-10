using Backend.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure.Extensions;

public static class InitializerExtensions
{
    public static async Task InitialiseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var databaseInitializer = scope.ServiceProvider.GetService<ApplicationDbContextInitializer>();
        if (databaseInitializer is not null)
        {
            await databaseInitializer.InitialiseAsync();
        }
    }
}
