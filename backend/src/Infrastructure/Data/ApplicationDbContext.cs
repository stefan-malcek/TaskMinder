using System.Diagnostics;
using System.Reflection;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Options;
using Backend.Domain.Entities;
using Backend.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Backend.Infrastructure.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IOptions<DbContextSettings> dbSettings)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options), IApplicationDbContext
{
    public DbSet<NoteList> NoteLists => Set<NoteList>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasPostgresExtension("unaccent");
        builder.HasPostgresExtension("citext");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (dbSettings.Value.EnableSensitiveLogging)
        {
            optionsBuilder.LogTo(o => Debug.WriteLine(o), LogLevel.Information)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging()
                .ConfigureWarnings(warningsActions =>
                {
                    warningsActions.Log(CoreEventId.FirstWithoutOrderByAndFilterWarning, CoreEventId.RowLimitingOperationWithoutOrderByWarning);
                });
        }
    }
}
