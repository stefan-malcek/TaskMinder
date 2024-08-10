using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Backend.Infrastructure.Identity;

/// <summary>
/// Identity user handling authentication and authorization.
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    [NotMapped]
    public new string Email
    {
        get => base.Email!;
        set => base.Email = value;
    }
}
