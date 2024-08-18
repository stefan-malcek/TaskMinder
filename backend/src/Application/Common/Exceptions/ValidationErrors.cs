namespace Backend.Application.Common.Exceptions;

public static class ValidationErrors
{
    // Generic Errors
    public const string ValidationFailed = nameof(ValidationFailed);
    public const string GenericError = nameof(GenericError);
    public const string InvalidUser = nameof(InvalidUser);

    // Auth
    public const string InvalidCredentials = nameof(InvalidCredentials);

    // Note Lists
    public const string InvalidNoteListParent = nameof(InvalidNoteListParent);

    private static readonly Dictionary<string, string> Errors
        = new()
        {
            // Generic Errors
            { ValidationFailed, "Validation Failed. See 'detail' or 'errors' for more details." },
            { GenericError, "Something went wrong." },
            { InvalidUser, "User is not valid." },
            // Auth
            { InvalidCredentials, "Email or password is wrong." },
            // Note Lists
            { InvalidNoteListParent, "Parent is invalid. Cannot be self referenced." },
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
