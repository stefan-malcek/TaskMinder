using System.Security.Claims;
using Backend.Application.Common.Interfaces;
using static System.Guid;

namespace Backend.Web.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public Guid? Id
    {
        get
        {
            string? userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return TryParse(userId, out var guid) ? guid : null;
        }
    }

    public string Language
    {
        get
        {
            return httpContextAccessor.HttpContext?.Request.Headers.AcceptLanguage.ToString() ?? "cs-CZ";
        }
    }
}
