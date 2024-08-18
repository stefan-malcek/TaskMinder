using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Models;
using Backend.Application.NoteLists.Commands;
using Backend.Application.NoteLists.Commands.CreateNoteList;
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
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        root.MapPost(CreateSubNoteListAsync, "{id}/NoteLists")
            .WithEndpointDescription("Create a new sub note list.", [
                ValidationErrors.ValidationFailed
            ])
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);
    }

    public async Task<CreatedEntityDto> CreateNoteListAsync(ISender sender, SaveNoteListDto saveNoteList)
    {
        CreateNoteListCommand command = new()
        {
            SaveNoteList = saveNoteList
        };
        Guid entityId = await sender.Send(command);
        return new CreatedEntityDto
        {
            Id = entityId
        };
    }

    public async Task<CreatedEntityDto> CreateSubNoteListAsync(ISender sender, Guid id, SaveNoteListDto saveNoteList)
    {
        CreateNoteListCommand command = new()
        {
            SaveNoteList = saveNoteList,
            ParentId = id
        };
        Guid entityId = await sender.Send(command);
        return new CreatedEntityDto
        {
            Id = entityId
        };
    }
}
