using System.Reflection;
using Backend.Web.Endpoints;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

namespace Backend.Web.Swagger;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddApiVersioning()
            .AddApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

        return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc($"v{WebApiVersion.One}",
                    new OpenApiInfo
                    {
                        Title = "Backend API",
                        Version = WebApiVersion.One.ToString()
                    });

                options.DocInclusionPredicate((name, api) => true);

                options.IncludeXmlComments(FormatPathForXmlDocumentation(typeof(Program)));
                options.IncludeXmlComments(FormatPathForXmlDocumentation(typeof(Application.DependencyInjection)));

                options.OperationFilter<AuthHeaderOperationFilter>();

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Type into the text box: Bearer {your JWT token}.",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
            })
        .AddFluentValidationRulesToSwagger();
    }

    private static string FormatPathForXmlDocumentation(Type fromAssembly)
    {
        var basePath = AppContext.BaseDirectory;
        var fileName = $"{fromAssembly.GetTypeInfo().Assembly.GetName().Name}.xml";
        return Path.Combine(basePath, fileName);
    }
}
