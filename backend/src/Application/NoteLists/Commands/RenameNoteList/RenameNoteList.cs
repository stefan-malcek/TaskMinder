﻿using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;

namespace Backend.Application.NoteLists.Commands.RenameNoteList;

public record RenameNoteListCommand : IRequest
{
    public required Guid Id { get; init; }
    public required SaveNoteListDto CreateNoteList { get; init; }
}

public class RenameNoteListCommandHandler(IApplicationDbContext context) : IRequestHandler<RenameNoteListCommand>
{
    public async Task Handle(RenameNoteListCommand request, CancellationToken cancellationToken)
    {
        SaveNoteListDto saveNoteList = request.CreateNoteList;
        NoteList? entity = await context.NoteLists
            .SingleOrDefaultAsync(n => n.Id == request.Id, cancellationToken);
        ThrowIf.Entity.IsNotFound(request.Id, typeof(NoteList));

        entity!.Title = saveNoteList.Title;
        await context.SaveChangesAsync(cancellationToken);
    }
}
