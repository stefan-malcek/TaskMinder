using Backend.Infrastructure.Identity;

namespace Backend.DataSeed.Factories;

public class ApplicationUserFactory
{
    public static Guid User1Id = new("019141b3-0df2-71ff-9c2f-38a038d3b738");
    public static string User1Email = new("user1@example.com");
    public static Guid User2Id = new("019141b5-0843-7532-91e1-2e2e74fdeff1");
    public static string User2Email = new("user2@example.com");
    public const string Password = "Passw0rd";
    public const string SecurityStamp = "E6CB3XCATOOD3UKWGTOW7SML4LJVUPF6";

    public static ICollection<ApplicationUser> Entities => new List<ApplicationUser>
    {
        NewEntity(User1Id, User1Email),
        NewEntity(User2Id, User2Email)
    };

    private static ApplicationUser NewEntity(Guid id, string email, bool confirmed = true)
    {
        return new ApplicationUser
        {
            Id = id,
            Email = email,
            UserName = email,
            EmailConfirmed = confirmed,
            SecurityStamp = SecurityStamp
        };
    }
}
