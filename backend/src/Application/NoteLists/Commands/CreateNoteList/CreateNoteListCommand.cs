using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;

namespace Backend.Application.NoteLists.Commands.CreateNoteList;

public record CreateNoteListCommand : IRequest<Guid>
{
    public Guid? ParentId { get; init; }
    public required SaveNoteListDto SaveNoteList { get; init; }
}

internal class CreateNoteListCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateNoteListCommand, Guid>
{
    public async Task<Guid> Handle(CreateNoteListCommand request, CancellationToken cancellationToken)
    {
        SaveNoteListDto saveNoteList = request.SaveNoteList;
        NoteList entity = new()
        {
            Title = saveNoteList.Title,
            ParentId = request.ParentId
        };
        context.NoteLists.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
