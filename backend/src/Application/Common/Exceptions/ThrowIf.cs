using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Ardalis.GuardClauses;
using Backend.Application.Common.Models;

namespace Backend.Application.Common.Exceptions;

public static class ThrowIf
{
    public static class Check
    {
        public static void Failed(bool succeeded, string error)
        {
            if (!succeeded)
            {
                throw new AppValidationException(error);
            }
        }

        public static void Failed(Result result, string error)
        {
            if (!result.Succeeded)
            {
                throw new AppValidationException(error, result.Errors);
            }
        }
    }

    public static class Entity
    {
        public static T IsNotFound<TKey, T>(
            [NotNull][ValidatedNotNull] TKey key,
            [NotNull][ValidatedNotNull] T? input,
            [CallerArgumentExpression("input")] string? parameterName = null) where TKey : struct
        {
            if (input is null)
            {
                throw new NotFoundException(key.ToString()!, parameterName!);
            }

            return input;
        }

        public static T IsNotFound<T>(
            [ValidatedNotNull] string key,
            [NotNull][ValidatedNotNull] T? input,
            [CallerArgumentExpression("input")] string? parameterName = null)
        {
            if (input is null)
            {
                throw new NotFoundException(key, parameterName!);
            }

            return input;
        }

        public static T IsInvalid<T, TKey>(
            [NotNull][ValidatedNotNull] TKey key,
            [NotNull][ValidatedNotNull] T? input,
            string error) where TKey : struct
        {
            if (input is null)
            {
                throw new AppValidationException(error, new[] { $"Value: ({key}) is not valid." });
            }

            return input;
        }
    }
}
