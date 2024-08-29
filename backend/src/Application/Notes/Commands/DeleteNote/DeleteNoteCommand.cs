using Ardalis.GuardClauses;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;

namespace Backend.Application.Notes.Commands.DeleteNote;

public record DeleteNoteCommand : IRequest
{
    public required Guid Id { get; init; }
}

internal class MoveNoteCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteNoteCommand>
{
    public async Task Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
    {
        Note? entity = await context.Notes
            .SingleOrDefaultAsync(n => n.Id == request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);

        context.Notes.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
    }
}
