using Backend.Application.Common.Options;

namespace Backend.Web.Infrastructure;

public static class OptionsExtension
{
    public static IServiceCollection AddWebApiOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<CorsSettings>(configuration.GetSection(nameof(CorsSettings)));
        services.Configure<DbContextSettings>(configuration.GetSection(nameof(DbContextSettings)));
        services.Configure<WebAppUrlSettings>(configuration.GetSection(nameof(WebAppUrlSettings)));
        services.Configure<EmailSenderSettings>(configuration.GetSection(nameof(EmailSenderSettings)));
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        return services;
    }
}

