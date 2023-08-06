using Microsoft.AspNetCore.Identity.UI.Services;

namespace Bulky.Utilities;
public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        // NotImplemented yet
        // implement logic to send email
        return Task.CompletedTask;
    }
}
