using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Models;
using Backend.Application.NoteLists.Commands;
using Backend.Application.NoteLists.Commands.CreateNoteList;
using Backend.Application.NoteLists.Commands.MoveNoteList;
using Backend.Application.NoteLists.Commands.RenameNoteList;
using Backend.Web.Infrastructure;

namespace Backend.Web.Endpoints;

public class NoteLists : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        var root = app.MapGroup(this)
            .RequireAuthorization();

        root.MapPost(CreateNoteListAsync)
            .WithEndpointDescription("Create a new root note list.", [
                ValidationErrors.ValidationFailed
            ])
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        root.MapPut(RenameNoteListAsync, "{id}/Rename")
            .WithEndpointDescription("Update the title of the given note list.", [
                ValidationErrors.ValidationFailed
            ])
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound);

        root.MapPut(MoveNoteListAsync, "{id}/Move")
            .WithEndpointDescription("Move the given note list.", [
                ValidationErrors.ValidationFailed
            ])
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound);

        root.MapPost(CreateSubNoteListAsync, "{id}/NoteLists")
            .WithEndpointDescription("Create a new sub note list for the given parent.", [
                ValidationErrors.ValidationFailed
            ])
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound);
    }

    public async Task<CreatedEntityDto> CreateNoteListAsync(ISender sender, SaveNoteListDto saveNoteList)
    {
        CreateNoteListCommand command = new() { SaveNoteList = saveNoteList };
        Guid entityId = await sender.Send(command);
        return new CreatedEntityDto { Id = entityId };
    }

    public async Task RenameNoteListAsync(ISender sender, Guid id, SaveNoteListDto saveNoteList)
    {
        RenameNoteListCommand command = new()
        {
            Id = id,
            SaveNoteList = saveNoteList
        };
        await sender.Send(command);
    }

    public async Task MoveNoteListAsync(ISender sender, Guid id, MoveNoteListDto moveNoteList)
    {
        MoveNoteListCommand command = new()
        {
            Id = id,
            MoveNoteList = moveNoteList
        };
        await sender.Send(command);
    }

    public async Task<CreatedEntityDto> CreateSubNoteListAsync(ISender sender, Guid id, SaveNoteListDto saveNoteList)
    {
        CreateNoteListCommand command = new()
        {
            SaveNoteList = saveNoteList,
            ParentId = id
        };
        Guid entityId = await sender.Send(command);
        return new CreatedEntityDto { Id = entityId };
    }
}
