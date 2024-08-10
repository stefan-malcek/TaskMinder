using Backend.Domain.Entities;

namespace Backend.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<NoteList> NoteLists { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
