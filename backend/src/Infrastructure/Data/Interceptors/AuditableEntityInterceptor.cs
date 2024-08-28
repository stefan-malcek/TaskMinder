using Backend.Application.Common.Interfaces;
using Backend.Domain.Common;
using Backend.Domain.Entities;
using Backend.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Backend.Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor(ICurrentUser user, TimeProvider dateTime) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        AuditProperties(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        AuditProperties(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void AuditProperties(DbContext? context)
    {
        if (context == null) return;

        var auditableEntries = context.ChangeTracker.Entries<BaseAuditableEntity>()
            .Where(x => x.State is EntityState.Added or EntityState.Deleted or EntityState.Modified)
            .Select(CreateTrailEntry)
            .ToList();

        context.AddRange(auditableEntries);
    }

    private AuditTrail CreateTrailEntry(EntityEntry<BaseAuditableEntity> entry)
    {
        var trailEntry = new AuditTrail
        {
            EntityName = entry.Entity.GetType().Name,
            UserId = user.Id,
            AuditedAt = dateTime.GetUtcNow()
        };

        SetAuditTrailPropertyValues(entry, trailEntry);
        SetAuditTrailNavigationValues(entry, trailEntry);
        SetAuditTrailReferenceValues(entry, trailEntry);

        return trailEntry;
    }

    private static void SetAuditTrailPropertyValues(EntityEntry entry, AuditTrail trailEntry)
    {
        // Skip temp fields (that will be assigned automatically by ef core engine, for example: when inserting an entity
        foreach (var property in entry.Properties.Where(x => !x.IsTemporary))
        {
            if (property.Metadata.IsPrimaryKey())
            {
                trailEntry.PrimaryKey = property.CurrentValue?.ToString();
                continue;
            }

            // Filter properties that should not appear in the audit list
            if (property.Metadata.Name.Equals("PasswordHash"))
            {
                continue;
            }

            SetAuditTrailPropertyValue(entry, trailEntry, property);
        }
    }

    private static void SetAuditTrailPropertyValue(EntityEntry entry, AuditTrail trailEntry, PropertyEntry property)
    {
        var propertyName = property.Metadata.Name;

        switch (entry.State)
        {
            case EntityState.Added:
                trailEntry.TrailType = TrailType.Create;
                trailEntry.NewValues[propertyName] = property.CurrentValue;

                break;

            case EntityState.Deleted:
                trailEntry.TrailType = TrailType.Delete;
                trailEntry.OldValues[propertyName] = property.OriginalValue;

                break;

            case EntityState.Modified:
                if (property.IsModified && (property.OriginalValue is null || !property.OriginalValue.Equals(property.CurrentValue)))
                {
                    trailEntry.ChangedColumns.Add(propertyName);
                    trailEntry.TrailType = TrailType.Update;
                    trailEntry.OldValues[propertyName] = property.OriginalValue;
                    trailEntry.NewValues[propertyName] = property.CurrentValue;
                }

                break;
        }

        if (trailEntry.ChangedColumns.Count > 0)
        {
            trailEntry.TrailType = TrailType.Update;
        }
    }

    private static void SetAuditTrailReferenceValues(EntityEntry entry, AuditTrail trailEntry)
    {
        // TODO: debug changing parent. In audit is stored reference name.
        foreach (var reference in entry.References.Where(x => x.IsModified))
        {
            var referenceName = reference.EntityEntry.Entity.GetType().Name;
            trailEntry.ChangedColumns.Add(referenceName);
        }
    }

    private static void SetAuditTrailNavigationValues(EntityEntry entry, AuditTrail trailEntry)
    {
        foreach (var navigation in entry.Navigations.Where(x => x.Metadata.IsCollection && x.IsModified))
        {
            if (navigation.CurrentValue is not IEnumerable<object> enumerable)
            {
                continue;
            }

            var collection = enumerable.ToList();
            if (collection.Count == 0)
            {
                continue;
            }

            var navigationName = collection.First().GetType().Name;
            trailEntry.ChangedColumns.Add(navigationName);
        }
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseCreatedAtEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = dateTime.GetUtcNow();
            }
        }

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            // In some cases we set IDs manually and need to preserve them
            // (e.g. connecting domain User to identity AspNetUser)
            if (entry.State == EntityState.Added)
            {
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
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}
