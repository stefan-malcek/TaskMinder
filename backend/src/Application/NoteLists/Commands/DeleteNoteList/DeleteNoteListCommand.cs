using Ardalis.GuardClauses;
using Backend.Application.Common.Extensions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;

namespace Backend.Application.NoteLists.Commands.DeleteNoteList;

public record DeleteNoteListCommand : IRequest
{
    public required Guid Id { get; init; }
}

internal class MoveNoteListCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteNoteListCommand>
{
    public async Task Handle(DeleteNoteListCommand request, CancellationToken cancellationToken)
    {
        NoteList? entity = await context.NoteLists
            .SingleOrDefaultAsync(n => n.Id == request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);

        bool hasDependencies = await context.NoteLists
            .Where(n => n.Id == request.Id)
            .Select(n => n.Children.Any() || n.Notes.Any())
            .SingleAsync(cancellationToken);
        Guard.Against.Conflict(() => !hasDependencies);

        context.NoteLists.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }
}
