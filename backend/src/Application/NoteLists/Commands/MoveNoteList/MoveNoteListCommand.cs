﻿using Ardalis.GuardClauses;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Extensions;
using Backend.Application.Common.Interfaces;
using Backend.Domain.Entities;

namespace Backend.Application.NoteLists.Commands.MoveNoteList;

public record MoveNoteListCommand : IRequest
{
    public required Guid Id { get; init; }
    public required MoveNoteListDto MoveNoteList { get; init; }
}

internal class MoveNoteListCommandHandler(IApplicationDbContext context) : IRequestHandler<MoveNoteListCommand>
{
    public async Task Handle(MoveNoteListCommand request, CancellationToken cancellationToken)
    {
        MoveNoteListDto moveNoteList = request.MoveNoteList;
        NoteList? entity = await context.NoteLists
            .SingleOrDefaultAsync(n => n.Id == request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, entity);
        Guard.Against.InvalidValidationRule(moveNoteList, x => x.ParentId != request.Id, ValidationErrors.InvalidNoteListParent);

        entity.ParentId = moveNoteList.ParentId;
        await context.SaveChangesAsync(cancellationToken);
    }
}
