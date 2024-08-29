using Ardalis.GuardClauses;
using Backend.Application.Common.Exceptions;

namespace Backend.Application.Common.Extensions;

internal static class GuardsExtensions
{
    public static T InvalidValidationRule<T>(this IGuardClause guardClause,
        T input,
        Func<T, bool> predicate,
        string error)
    {
        if (!predicate(input))
        {
            throw new ValidationRuleException(error);
        }

        return input;
    }

    public static void Conflict(this IGuardClause guardClause, Func<bool> predicate)
    {
        if (!predicate())
        {
            throw new ConflictException();
        }
    }
}
