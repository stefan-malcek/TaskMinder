using System.Diagnostics;
using Backend.DataSeed.Factories;
using Backend.Infrastructure.Data;
using Backend.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.DataSeed.Extensions;

public static class SeederExtensions
{
    public static Task SeedEntitiesAsync(this ApplicationDbContext context)
    {
        return Task.CompletedTask;
    }

    public static async Task SeedUsersAsync(this UserManager<ApplicationUser> userManager)
    {
        foreach (var user in ApplicationUserFactory.Entities)
        {
            await userManager.CreateAsync(user, ApplicationUserFactory.Password);
        }
    }

    public static void SeedEntities(this ApplicationDbContext context)
    {
        context.SeedEntitiesAsync().GetAwaiter().GetResult();
    }


    public static async Task SeedUsersAsync(
        this UserStore<ApplicationUser,
            IdentityRole<Guid>,
            ApplicationDbContext,
            Guid> userManager,
        PasswordHasher<ApplicationUser> hasher)
    {
        foreach (var user in ApplicationUserFactory.Entities)
        {
            var hash = hasher.HashPassword(user, ApplicationUserFactory.Password);
            user.PasswordHash = hash;
            await userManager.CreateAsync(user);
        }
    }

    private static async Task SeedEntityAsync<TEntity>(this DbContext context, IEnumerable<TEntity> data)
        where TEntity : class
    {
        try
        {
            context.Set<TEntity>().AddRange(data);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            // Report seeding failure.
            Debug.WriteLine(e);
            throw;
        }

        // Detach entities so change tracker is empty.
        var entries = context.ChangeTracker.Entries().ToList();
        foreach (var entry in entries)
        {
            entry.State = EntityState.Detached;
        }
    }
}
