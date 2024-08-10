using System.Text.Json.Serialization;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Options;
using Backend.Infrastructure.Data;
using Backend.Web.Infrastructure;
using Backend.Web.Services;
using Backend.Web.Swagger;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddWebApiOptions(configuration);

        var corsSettings = configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>();
        ArgumentNullException.ThrowIfNull(corsSettings);

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(corsSettings.Origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddTransient<IAuthTokenService, AuthTokenService>();

        services.AddHttpContextAccessor();

        services.AddAuth(configuration);

        services.ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        services.Configure<JsonOptions>(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddExceptionHandler<CustomExceptionHandler>();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

        services.AddSwaggerDocumentation();

        services.AddApplicationInsightsTelemetry();

        return services;
    }
}
