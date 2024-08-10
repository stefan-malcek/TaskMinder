using FluentValidation.Results;

namespace Backend.Application.Common.Exceptions;

public class ValidationException(string errorName) : Exception
{
    public ValidationException(string errorName, IEnumerable<ValidationFailure> failures)
        : this(errorName)
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    public ValidationException(string errorName, IEnumerable<string> failures)
        : this(errorName)
    {
        Errors = new Dictionary<string, string[]> { { "BusinessError", failures.ToArray() } };
    }

    public string ErrorName { get; } = errorName;
    public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]> { { "BusinessError", [ValidationErrors.GetDescription(errorName)] } };
}
