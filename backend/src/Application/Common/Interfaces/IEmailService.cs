using Backend.Application.Common.Models.Identity;

namespace Backend.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendConfirmationLinkAsync(UserViewModel user, string code);
    Task SendPasswordResetLinkAsync(UserViewModel user, string code);
}
