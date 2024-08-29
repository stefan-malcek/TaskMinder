using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;

namespace Backend.Application.Notes.Commands.CreateNote;

public record CreateNoteCommand : IRequest<Guid>
{
    public required Guid ListId { get; init; }
    public required SaveNoteDto SaveNote { get; init; }
}

internal class CreateNoteCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateNoteCommand, Guid>
{
    public async Task<Guid> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
    {
        SaveNoteDto saveNote = request.SaveNote;
        Note entity = new()
        {
            ListId = request.ListId,
            Title = saveNote.Title,
            Content = saveNote.Content
        };
        context.Notes.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
