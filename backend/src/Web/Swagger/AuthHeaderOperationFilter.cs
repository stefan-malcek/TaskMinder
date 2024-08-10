using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Backend.Web.Swagger;

public class AuthHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var filterDescriptor = context.ApiDescription.ActionDescriptor.EndpointMetadata;
        var allowAnonymous = filterDescriptor.Any(filter => filter is AllowAnonymousAttribute);
        if (allowAnonymous)
        {
            return;
        }

        var requireAuthorize = filterDescriptor.Any(filter => filter is AuthorizeAttribute);
        if (!requireAuthorize)
        {
            return;
        }

        operation.Parameters ??= new List<OpenApiParameter>();
        operation.Security = new List<OpenApiSecurityRequirement>();
        operation.Security.Add(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }});
    }
}
