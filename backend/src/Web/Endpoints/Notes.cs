using Backend.Application.Common.Exceptions;
using Backend.Application.Notes.Commands;
using Backend.Application.Notes.Commands.DeleteNote;
using Backend.Application.Notes.Commands.MoveNote;
using Backend.Application.Notes.Commands.UpdateNote;
using Backend.Web.Infrastructure;

namespace Backend.Web.Endpoints;

public class Notes : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        var root = app.MapGroup(this)
            .RequireAuthorization();

        root.MapPut(UpdateNoteAsync, "{id}")
            .WithEndpointDescription("Update the given note.", [
                ValidationErrors.ValidationFailed
            ])
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound);

        root.MapPut(MoveNoteAsync, "{id}/Move")
            .WithEndpointDescription("Move the given note.", [
                ValidationErrors.ValidationFailed
            ])
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound);

        root.MapDelete(DeleteNoteAsync, "{id}")
            .WithEndpointDescription("Delete the given note.", [
                ValidationErrors.ValidationFailed
            ])
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    public async Task UpdateNoteAsync(ISender sender, Guid id, SaveNoteDto saveNote)
    {
        UpdateNoteCommand command = new()
        {
            Id = id,
            SaveNote = saveNote
        };
        await sender.Send(command);
    }

    public async Task MoveNoteAsync(ISender sender, Guid id, MoveNoteDto moveNote)
    {
        MoveNoteCommand command = new()
        {
            Id = id,
            MoveNote = moveNote
        };
        await sender.Send(command);
    }

    public async Task DeleteNoteAsync(ISender sender, Guid id)
    {
        DeleteNoteCommand command = new() { Id = id };
        await sender.Send(command);
    }
}
