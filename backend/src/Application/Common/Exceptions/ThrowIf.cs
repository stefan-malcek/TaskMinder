using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Ardalis.GuardClauses;
using Backend.Application.Common.Models;

namespace Backend.Application.Common.Exceptions;

public static class ThrowIf
{
    public static class Registration
    {
        public static void Failed(Result result)
        {
            if (result.Succeeded)
            {
                return;
            }

            if (result.Errors.Any(a => a.Contains("Password")))
            {
                throw new ValidationException(ValidationErrors.WeakPassword, result.Errors);
            }

            throw new ValidationException(ValidationErrors.RegistrationFailed, result.Errors);
        }
    }

    public static class EmailConfirmation
    {
        public static void Failed(Result result)
        {
            if (result.Succeeded)
            {
                return;
            }

            throw new ValidationException(ValidationErrors.EmailConfirmationFailed, result.Errors);
        }
    }

    public static class PasswordReset
    {
        public static void Failed(Result result)
        {
            if (result.Succeeded)
            {
                return;
            }

            if (result.Errors.Any(a => a.Contains("Password")))
            {
                throw new ValidationException(ValidationErrors.WeakPassword, result.Errors);
            }

            throw new ValidationException(ValidationErrors.PasswordResetFailed, result.Errors);
        }
    }

    public static class Check
    {
        public static void Failed(bool succeeded, string error)
        {
            if (!succeeded)
            {
                throw new ValidationException(error);
            }
        }

        public static void Failed(Result result, string error)
        {
            if (!result.Succeeded)
            {
                throw new ValidationException(error, result.Errors);
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
                throw new ValidationException(error, new[] { $"Value: ({key}) is not valid." });
            }

            return input;
        }
    }
}
