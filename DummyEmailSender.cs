using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace AI_based_Language_Teaching
{
    public class DummyEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Just log or skip for now
            Console.WriteLine($"Pretending to send email to {email}");
            return Task.CompletedTask;
        }
    }
}
