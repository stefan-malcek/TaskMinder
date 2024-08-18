using Ardalis.GuardClauses;
using Backend.Application.Common.Exceptions;

namespace Backend.Application.Common.Extensions;

internal static class GuardsExtensions
{
    public static T InvalidBusinessRule<T>(this IGuardClause guardClause,
        T input,
        Func<T, bool> predicate,
        string error)
    {
        if (!predicate(input))
        {
            throw new AppValidationException(error);
        }

        return input;
    }
}
