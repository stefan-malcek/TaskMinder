using Backend.Application.Common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Backend.Infrastructure.Email;
internal class SendGridEmailService(
    IOptions<EmailSenderSettings> emailSettings,
    ILogger<SendGridEmailService> logger)
    : EmailServiceBase(emailSettings)
{
    protected override async Task SendEmailAsync(string toEmail, string subject, string content)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(_emailSettings.Value.AuthKey);
        var client = new SendGridClient(_emailSettings.Value.AuthKey);
        var message = new SendGridMessage
        {
            From = new EmailAddress(_emailSettings.Value.DefaultSenderEmail, _emailSettings.Value.AppName),
            Subject = subject,
            PlainTextContent = content,
            HtmlContent = content
        };
        message.AddTo(new EmailAddress(toEmail));

        var response = await client.SendEmailAsync(message);
        if (response.IsSuccessStatusCode)
        {
            logger.LogInformation("Email queued successfully");
        }
        else
        {
            logger.LogError("Failed to send email with {@Data}", response);
        }
    }
}
