using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;

namespace Backend.Application.NoteLists.Commands.CreateNoteList;

public record CreateNoteListCommand : IRequest<Guid>
{
    public string Title { get; init; } = null!;
}

public class CreateNoteListCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateNoteListCommand, Guid>
{
    public async Task<Guid> Handle(CreateNoteListCommand request, CancellationToken cancellationToken)
    {
        var entity = new NoteList
        {
            Title = request.Title
        };

        context.NoteLists.Add(entity);
        await context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
