namespace Backend.Application.Common.Models;

public class KeyValuePairDtoSwaggerDoc : AbstractValidator<KeyValuePairDto>
{
    public KeyValuePairDtoSwaggerDoc()
    {
        RuleFor(m => m.Id).NotNull();
        RuleFor(m => m.Value).NotNull();
    }
}
