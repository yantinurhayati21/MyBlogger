using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MailKit.Net.Smtp;
using MyBlogger.Models;

namespace MyBlogger.Services
{
    public class EmailService: IEmailService
    {
        private readonly EmailSetting _setting;
        public EmailService(EmailSetting e)
        {
            _setting = e;
        }
        public void SendEmail(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_setting.From));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Plain)
            {
                Text = body
            };

            using var smtp = new SmtpClient();
            smtp.Connect(_setting.Host, _setting.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(
                _setting.Username,
                _setting.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
