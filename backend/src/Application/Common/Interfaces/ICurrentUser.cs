namespace Backend.Application.Common.Interfaces;

public interface ICurrentUser
{
    /// <summary>
    /// Currently logged user.
    /// </summary>
    Guid? Id { get; }
    /// <summary>
    /// Request language from AcceptLanguage header. Default to cs-CZ.
    /// </summary>
    string Language { get; }
}
