using Ardalis.GuardClauses;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Extensions;
using Backend.Application.Common.Interfaces;

namespace Backend.Application.Auth.Commands.Login;

public record LoginCommand : IRequest<LoggedUserDto>
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}

internal class LoginCommandHandler(IIdentityService identityService, IAuthTokenService authTokenService)
    : IRequestHandler<LoginCommand, LoggedUserDto>
{
    public async Task<LoggedUserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await identityService.VerifyUserAsync(request.Email, request.Password);
        Guard.Against.InvalidValidationRule(user, x => x is not null, ValidationErrors.InvalidCredentials);

        var token = authTokenService.GenerateToken(user!.Id);
        return new LoggedUserDto
        {
            Token = token,
            Id = user.Id,
            Email = user.Email
        };
    }
}
