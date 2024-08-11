using Backend.Application.Auth.Commands.Login;
using Backend.Application.Common.Exceptions;
using Backend.Web.Infrastructure;

namespace Backend.Web.Endpoints;

public class Auth : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        var anonymousRoot = app.MapGroup(this);

        anonymousRoot.MapPost(LoginAsync, WebApiRoutes.LoginPath)
            .Produces(StatusCodes.Status200OK)
            .WithEndpointDescription("Sign a user in.", [
                ValidationErrors.ValidationFailed,
                ValidationErrors.InvalidCredentials
            ])
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }

    public async Task<LoggedUserDto> LoginAsync(ISender sender, LoginCommand loginCommand)
    {
        return await sender.Send(loginCommand);
    }
}
