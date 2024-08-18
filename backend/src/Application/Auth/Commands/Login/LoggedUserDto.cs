namespace Backend.Application.Auth.Commands.Login;

public class LoggedUserDto
{
    /// <summary>
    /// JWT Token
    /// </summary>
    public required string Token { get; init; }
    /// <summary>
    /// User identifier
    /// </summary>
    public required Guid Id { get; init; }
    public required string Email { get; init; }
}
