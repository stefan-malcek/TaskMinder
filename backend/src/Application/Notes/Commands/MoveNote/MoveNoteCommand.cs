using Ardalis.GuardClauses;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;

namespace Backend.Application.Notes.Commands.MoveNote;

public record MoveNoteCommand : IRequest
{
    public required Guid Id { get; init; }
    public required MoveNoteDto MoveNote { get; init; }
}

internal class MoveNoteCommandHandler(IApplicationDbContext context) : IRequestHandler<MoveNoteCommand>
{
    public async Task Handle(MoveNoteCommand request, CancellationToken cancellationToken)
    {
        MoveNoteDto moveNote = request.MoveNote;
        Note? entity = await context.Notes
            .SingleOrDefaultAsync(n => n.Id == request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);

        entity.ListId = moveNote.ListId;
        await context.SaveChangesAsync(cancellationToken);
    }
}
