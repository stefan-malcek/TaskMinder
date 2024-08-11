using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.Auth.Commands.Login;

public record LoginCommand : IRequest<LoggedUserDto>
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}

public class LoginCommandHandler(IIdentityService identityService, IAuthTokenService authTokenService)
    : IRequestHandler<LoginCommand, LoggedUserDto>
{
    public async Task<LoggedUserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await identityService.VerifyUserAsync(request.Email, request.Password);
        ThrowIf.Check.Failed(user is not null, ValidationErrors.InvalidCredentials);

        var token = authTokenService.GenerateToken(user!.Id);
        return new LoggedUserDto
        {
            Token = token,
            Id = user.Id,
            Email = user.Email
        };
    }
}
