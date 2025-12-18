using MagFlow.BLL.Services.Interfaces;
using MagFlow.Domain.Core;
using MagFlow.Shared.Models.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace MagFlow.BLL.Services
{
    public class EmailService : IEmailService
    {
        private ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
        {
            
        }

        public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
        {
           
        }

        public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
        {
            
        }

        public async Task SendToMeAsync(string subject, MimeEntity body)
        {
            await SendAsync(AppSettings.SmtpSettings?.Username?? "", AppSettings.SmtpSettings?.Email ?? "", subject, body);
        }

        public async Task SendAsync(string receiverName, string receiverEmail, string subject, MimeEntity body)
        {
            try
            {
                if (AppSettings.SmtpSettings == null || string.IsNullOrEmpty(AppSettings.SmtpSettings.Server) || AppSettings.SmtpSettings.Port == 0 ||
                    string.IsNullOrEmpty(AppSettings.SmtpSettings.Email) || string.IsNullOrEmpty(AppSettings.SmtpSettings.Password))
                    return;
                if (string.IsNullOrEmpty(receiverEmail))
                    return;

                var server = AppSettings.SmtpSettings.Server;
                var port = AppSettings.SmtpSettings.Port;
                var ssl = AppSettings.SmtpSettings.SSLEnabled;
                var senderName = AppSettings.SmtpSettings.Username;
                var senderEmail = AppSettings.SmtpSettings.Email;
                var password = AppSettings.SmtpSettings.Password;

                if (string.IsNullOrEmpty(receiverName))
                    receiverName = receiverEmail;
                if (string.IsNullOrEmpty(senderName))
                    senderName = senderEmail;

                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(senderName, senderEmail));
                email.To.Add(new MailboxAddress(receiverName, receiverEmail));
                email.Subject = subject;
                email.Body = body;

                using (var smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync(server, port, ssl);
                    await smtp.AuthenticateAsync(senderEmail, password);

                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while sending email: {ex}");
            }
        }
    }
}
