using Ardalis.GuardClauses;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;

namespace Backend.Application.NoteLists.Commands.RenameNoteList;

public record RenameNoteListCommand : IRequest
{
    public required Guid Id { get; init; }
    public required SaveNoteListDto SaveNoteList { get; init; }
}

internal class RenameNoteListCommandHandler(IApplicationDbContext context) : IRequestHandler<RenameNoteListCommand>
{
    public async Task Handle(RenameNoteListCommand request, CancellationToken cancellationToken)
    {
        SaveNoteListDto saveNoteList = request.SaveNoteList;
        NoteList? entity = await context.NoteLists
            .SingleOrDefaultAsync(n => n.Id == request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);

        entity.Title = saveNoteList.Title;
        await context.SaveChangesAsync(cancellationToken);
    }
}
