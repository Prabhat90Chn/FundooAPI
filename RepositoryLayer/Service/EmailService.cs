using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interface;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class EmailService : IEmailSender
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _senderEmail;
        private readonly string _password;

        public EmailService(IConfiguration config)
        {
            _senderEmail = config["SmtpSetting:SenderEmail"];
            _password = config["SmtpSetting:Password"];

            _smtpClient = new SmtpClient(config["SmtpSetting:Server"])
            {
                EnableSsl = bool.Parse(config["SmtpSetting:SSL"]),
                UseDefaultCredentials = bool.Parse(config["SmtpSetting:DefaultCredentials"]),
                Credentials = new NetworkCredential(_senderEmail, _password),
                Port = int.Parse(config["SmtpSetting:Port"]),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }

        public async Task SendEmailAsync(string recipientEmail, string recipientName, string link)
        {
            var mailMessage = new MailMessage(_senderEmail, recipientEmail, recipientName, link)
            {
                IsBodyHtml = false
            };

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
