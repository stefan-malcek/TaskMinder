using Backend.Domain.Entities;

namespace Backend.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    // Notes
    DbSet<NoteList> NoteLists { get; }
    DbSet<Note> Notes { get; }

    // Audit
    DbSet<AuditTrail> AuditTrails { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
