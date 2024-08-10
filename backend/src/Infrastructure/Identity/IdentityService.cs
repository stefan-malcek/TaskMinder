using System.Text;
using Backend.Application.Common.Exceptions;
using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Models;
using Backend.Application.Common.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using UUIDNext;

namespace Backend.Infrastructure.Identity;

public class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
{
    public async Task<string?> GetUserEmailAsync(Guid userId)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        return user?.Email;
    }

    public async Task<UserViewModel?> GetUserAsync(Guid userId)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        return user is null
            ? null
            : new UserViewModel(user.Id, user.Email, user.EmailConfirmed);
    }

    public async Task<UserViewModel?> GetUserByEmailAsync(string email)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user is null
            ? null
            : new UserViewModel(user.Id, email, user.EmailConfirmed);
    }

    public async Task<string> GeneratePasswordResetTokenAsync(Guid id)
    {
        var user = await userManager.Users.FirstAsync(u => u.Id == id);
        var code = await userManager.GeneratePasswordResetTokenAsync(user);
        return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
    }

    public async Task<Result> ResetPasswordAsync(Guid userId, string code, string password)
    {
        var user = await userManager.Users.FirstAsync(u => u.Id == userId);

        try
        {
            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await userManager.ResetPasswordAsync(user, decodedCode, password);

            return result.ToApplicationResult();
        }
        catch (FormatException)
        {
            return IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken()).ToApplicationResult();
        }
    }

    public async Task<bool> IsEmailTakenAsync(string email)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(u => u.Email == email);
        return user is not null;
    }

    public async Task<(Result Result, UserViewModel User)> CreateClientUserAsync(string email, string password)
    {
        var user = new ApplicationUser { Id = Uuid.NewDatabaseFriendly(Database.PostgreSql), UserName = email, Email = email };
        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return (result.ToApplicationResult(), new UserViewModel(user.Id, email, user.EmailConfirmed));
        }

        return (result.ToApplicationResult(), new UserViewModel(user.Id, email, user.EmailConfirmed));
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(Guid userId)
    {
        var user = await userManager.Users.FirstAsync(u => u.Id == userId);
        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
    }

    public async Task<Result> ConfirmEmailAsync(Guid userId, string code)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
        ThrowIf.Entity.IsInvalid(userId, user, ValidationErrors.InvalidUser);

        var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        IdentityResult result = await userManager.ConfirmEmailAsync(user, decodedCode);
        return result.ToApplicationResult();
    }

    public async Task<UserViewModel?> VerifyUserAsync(string email, string password)
    {
        var user = userManager.Users.SingleOrDefault(u => u.Email == email);
        if (user is null)
        {
            return null;
        }

        if (!await userManager.CheckPasswordAsync(user, password))
        {
            return null;
        }

        return new UserViewModel(user.Id, email, user.EmailConfirmed);
    }
}
