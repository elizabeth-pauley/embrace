using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace Embrace.Services
{
    public class MockEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // For now, just log the details
            Console.WriteLine($"Mock email sent to {email}: {subject} - {htmlMessage}");
            return Task.CompletedTask;
        }
    }
}
