using Backend.Application.Common.Interfaces;
using Backend.Application.Common.Models.Identity;
using Backend.Application.Common.Options;
using Microsoft.Extensions.Options;

namespace Backend.Infrastructure.Email;

public abstract class EmailServiceBase(IOptions<EmailSenderSettings> emailSettings) : IEmailService
{
    protected readonly IOptions<EmailSenderSettings> _emailSettings = emailSettings;

    public async Task SendConfirmationLinkAsync(UserViewModel user, string code)
    {
        await SendEmailAsync(user.Email, "Hi", "Content");
    }

    public async Task SendPasswordResetLinkAsync(UserViewModel user, string code)
    {
        await SendEmailAsync(user.Email, "Hi", "Content");
    }

    protected abstract Task SendEmailAsync(string toEmail, string subject, string content);
}
