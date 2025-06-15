using Microsoft.Extensions.Configuration;
using Sakenny.Core.DTO;
using Sakenny.Core.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;


namespace Sakenny.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task SendEmail(EmailDto emailDto)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(configuration["Email:EmailHost"]));
            email.To.Add(MailboxAddress.Parse(emailDto.To));
            email.Subject = emailDto.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = emailDto.Body };
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(configuration["Email:Server"], 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(configuration["Email:EmailHost"], configuration["Email:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
