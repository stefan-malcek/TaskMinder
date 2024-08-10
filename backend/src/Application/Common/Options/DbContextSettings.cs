namespace Backend.Application.Common.Options;

public record DbContextSettings
{
    public bool EnableSensitiveLogging { get; set; } = false;
}
