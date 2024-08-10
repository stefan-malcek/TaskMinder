using Backend.Application.Common.Interfaces;
using Backend.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UUIDNext;

namespace Backend.Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor(ICurrentUser user, TimeProvider dateTime) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseCreatedAtEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                // In some cases we set IDs manually and need to preserve them
                // (e.g. connecting domain User to identity AspNetUser)
                if (entry.Entity.Id == default)
                {
                    entry.Entity.Id = Uuid.NewDatabaseFriendly(Database.PostgreSql);
                }

                entry.Entity.CreatedAt = dateTime.GetUtcNow();
            }
        }

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            // In some cases we set IDs manually and need to preserve them
            // (e.g. connecting domain User to identity AspNetUser)
            if (entry.State == EntityState.Added)
            {
                if (entry.Entity.Id == default)
                {
                    entry.Entity.Id = Uuid.NewDatabaseFriendly(Database.PostgreSql);
                }

                entry.Entity.CreatedBy = user?.Id;
                entry.Entity.CreatedAt = dateTime.GetUtcNow();
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedBy = user?.Id;
                entry.Entity.LastModifiedAt = dateTime.GetUtcNow();
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}
