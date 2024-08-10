namespace Backend.Application.Common.Options;

public record EmailSenderSettings
{
    public required string DefaultSenderEmail { get; init; }
    public required string AppName { get; init; }
    public required string AuthKey { get; init; }
}
