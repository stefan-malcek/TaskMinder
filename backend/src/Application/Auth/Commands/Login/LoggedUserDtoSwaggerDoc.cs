namespace Backend.Application.Auth.Commands.Login;

public class LoggedUserDtoSwaggerDoc : AbstractValidator<LoggedUserDto>
{
    public LoggedUserDtoSwaggerDoc()
    {
        RuleFor(r => r.Token)
            .NotEmpty();

        RuleFor(r => r.Id)
            .NotNull();

        RuleFor(r => r.Email)
            .NotEmpty();
    }
}
