namespace Backend.Application.Common.Options;
public record WebAppUrlSettings
{
    public required string BaseUrl { get; init; }
    public required string ConfirmEmailSubUrl { get; init; }
    public required string ResetPasswordSubUrl { get; init; }
}
