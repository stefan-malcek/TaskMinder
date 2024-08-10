using Backend.Application.Common.Models;
using Backend.Application.Common.Models.Identity;

namespace Backend.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserEmailAsync(Guid userId);
    Task<UserViewModel?> GetUserByEmailAsync(string email);
    Task<UserViewModel?> GetUserAsync(Guid userId);
    Task<bool> IsEmailTakenAsync(string email);
    Task<(Result Result, UserViewModel User)> CreateClientUserAsync(string email, string password);
    Task<string> GenerateEmailConfirmationTokenAsync(Guid userId);
    Task<string> GeneratePasswordResetTokenAsync(Guid id);
    Task<Result> ResetPasswordAsync(Guid userId, string code, string password);
    Task<Result> ConfirmEmailAsync(Guid userId, string code);
    Task<UserViewModel?> VerifyUserAsync(string email, string password);
}
