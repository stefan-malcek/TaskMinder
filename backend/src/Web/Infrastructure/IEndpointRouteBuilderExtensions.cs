using System.Text;
using Backend.Application.Common.Exceptions;

namespace Backend.Web.Infrastructure;

public static class RouteGroupBuilderExtensions
{
    public static RouteHandlerBuilder MapGet(this RouteGroupBuilder builder, Delegate handler, string pattern = "")
    {
        Guard.Against.AnonymousMethod(handler);

        return builder.MapGet(pattern, handler)
            .WithName(handler.Method.Name)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    public static RouteHandlerBuilder MapPost(this RouteGroupBuilder builder, Delegate handler, string pattern = "")
    {
        Guard.Against.AnonymousMethod(handler);

        return builder.MapPost(pattern, handler)
                .WithName(handler.Method.Name)
                .ProducesValidationProblem()
                .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    public static RouteHandlerBuilder MapPut(this RouteGroupBuilder builder, Delegate handler, string pattern = "")
    {
        Guard.Against.AnonymousMethod(handler);

        return builder.MapPut(pattern, handler)
            .WithName(handler.Method.Name)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    public static RouteHandlerBuilder MapPatch(this RouteGroupBuilder builder, Delegate handler, string pattern = "")
    {
        Guard.Against.AnonymousMethod(handler);

        return builder.MapPatch(pattern, handler)
            .WithName(handler.Method.Name)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    public static RouteHandlerBuilder MapDelete(this RouteGroupBuilder builder, Delegate handler, string pattern)
    {
        Guard.Against.AnonymousMethod(handler);

        return builder.MapDelete(pattern, handler)
            .WithName(handler.Method.Name)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }

    public static RouteHandlerBuilder WithEndpointDescription(this RouteHandlerBuilder builder, string? description = null, string[]? errors = null)
    {
        var stringBuilder = new StringBuilder(description ?? string.Empty);

        if (errors == null || errors.Length == 0)
        {
            return builder.WithDescription(stringBuilder.ToString());
        }

        stringBuilder.Append("\n### Error details for HTTP Status 400 BadRequest:\n");

        var validationErrorDocument = errors.Aggregate(string.Empty,
                (current, error) => current + "+ **" + error + "** - " + ValidationErrors.GetDescription(error) + "\n");
        stringBuilder.Append(validationErrorDocument);

        return builder.WithDescription(stringBuilder.ToString());
    }
}
