namespace Backend.Application.Common.Options;
public record CorsSettings
{
    public required string[] Origins { get; init; }
}
