using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace IMS.Infrastructure.ExternalServices
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly SmtpClient _smtpClient;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;

            var smtpSettings = _configuration.GetSection("SmtpSettings");
            _smtpClient = new SmtpClient(smtpSettings["Host"])
            {
                Port = int.Parse(smtpSettings["Port"]),
                Credentials = new NetworkCredential(smtpSettings["Email"], smtpSettings["Password"]),
                EnableSsl = true,
            };
        }




        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings["Email"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(toEmail);

                await _smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }

        public void Dispose()
        {
            _smtpClient.Dispose();
        }
    }

}
