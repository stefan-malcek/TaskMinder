namespace Backend.Application.Common.Options;
public class JwtSettings
{
    public required string Secret { get; init; }
    public required TimeSpan Expiration { get; init; }
}
