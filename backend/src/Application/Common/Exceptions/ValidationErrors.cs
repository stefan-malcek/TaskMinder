namespace Backend.Application.Common.Exceptions;

public static class ValidationErrors
{
    // Generic Errors
    public const string ValidationFailed = nameof(ValidationFailed);
    public const string GenericError = nameof(GenericError);
    public const string InvalidUser = nameof(InvalidUser);

    // Auth
    public const string RegistrationFailed = nameof(RegistrationFailed);
    public const string WeakPassword = nameof(WeakPassword);
    public const string EmailNotUnique = nameof(EmailNotUnique);
    public const string EmailConfirmationFailed = nameof(EmailConfirmationFailed);
    public const string SignInFailed = nameof(SignInFailed);
    public const string EmailNotConfirmed = nameof(EmailNotConfirmed);
    public const string PasswordResetFailed = nameof(PasswordResetFailed);

    private static readonly Dictionary<string, string> Errors
        = new()
        {
            // Generic Errors
            { ValidationFailed, "Validation Failed. See 'detail' or 'errors' for more details." },
            { GenericError, "Something went wrong." },
            { InvalidUser, "User is not valid." },
            // Auth
            { RegistrationFailed, "Registration did not succeed." },
            { WeakPassword, "Password does not match conditions: 8 chars, at least one number, at least one upper char and at least one lower case char." },
            { EmailNotUnique, "Email is already taken." },
            { EmailConfirmationFailed, "Email confirmation failed." },
            { SignInFailed, "Sign in attempt failed." },
            { EmailNotConfirmed, "Email hasn't been confirmed by user." },
            { PasswordResetFailed, "Password reset failed." }
        };

    public static string GetDescription(string error)
    {
        return Errors.GetValueOrDefault(error) ?? "Unknown or undefined error.";
    }

    public static string GetErrorCode(string error)
    {
        return Errors.ContainsKey(error) ? error : ValidationFailed;
    }
}
