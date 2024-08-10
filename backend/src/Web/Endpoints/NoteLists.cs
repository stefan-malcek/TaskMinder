using Backend.Application.Common.Exceptions;
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
            .WithEndpointDescription("Create a new note list.", [
                ValidationErrors.ValidationFailed
            ])
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);
    }

    public async Task CreateNoteListAsync(ISender sender, CreateNoteListCommand createNoteList)
    {
        await sender.Send(createNoteList);
    }
}
