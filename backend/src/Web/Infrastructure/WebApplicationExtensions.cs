using System.Reflection;
using Backend.Web.Endpoints;

namespace Backend.Web.Infrastructure;

public static class WebApplicationExtensions
{
    public static RouteGroupBuilder MapGroup(this WebApplication app, EndpointGroupBase group)
    {
        var groupName = group.GetType().Name;
        var apiVersionSet = app.NewApiVersionSet(groupName)
            .Build();

        return app
            .MapGroup($"/api/v{{version:apiVersion}}/{groupName}")
            .WithGroupName(groupName)
            .WithTags(groupName)
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(WebApiVersion.One)
            .WithOpenApi();
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var endpointGroupType = typeof(EndpointGroupBase);
        var assembly = Assembly.GetExecutingAssembly();
        var endpointGroupTypes = assembly.GetExportedTypes()
            .Where(t => t.IsSubclassOf(endpointGroupType));

        foreach (var type in endpointGroupTypes)
        {
            if (Activator.CreateInstance(type) is EndpointGroupBase instance)
            {
                instance.Map(app);
            }
        }

        return app;
    }
}
