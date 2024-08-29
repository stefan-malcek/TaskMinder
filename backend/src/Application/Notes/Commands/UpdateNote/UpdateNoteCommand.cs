using Ardalis.GuardClauses;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;

namespace Backend.Application.Notes.Commands.UpdateNote;

public record UpdateNoteCommand : IRequest
{
    public required Guid Id { get; init; }
    public required SaveNoteDto SaveNote { get; init; }
}

internal class RenameNoteListCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateNoteCommand>
{
    public async Task Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
    {
        SaveNoteDto saveNote = request.SaveNote;
        Note? entity = await context.Notes
            .SingleOrDefaultAsync(n => n.Id == request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);

        entity.Title = saveNote.Title;
        entity.Content = saveNote.Content;
        await context.SaveChangesAsync(cancellationToken);
    }
}
