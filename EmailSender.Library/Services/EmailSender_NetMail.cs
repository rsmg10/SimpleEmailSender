using EmailSender.Library.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace EmailSender.Library.Services
{
    public class EmailSender_NetMail : IEmailSender
    {
        private readonly EmailSenderOptions _options;
        public EmailSender_NetMail(EmailSenderOptions options) => _options = options;
        public EmailSender_NetMail(IOptions<EmailSenderOptions> options) => _options = options.Value;

        public bool SendEmail(Email Email)
        {
            try
            {
                var message = GetMessage(Email);
                SmtpClient client = GetClient();
                client.Send(message);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        private SmtpClient GetClient()
        {
            var client = new SmtpClient();
            client.Host = _options.EmailDomain;
            client.Port = _options.Port;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_options.Email, _options.AppPassword);
            return client;
        }
        private static MailMessage GetMessage(Email Email)
        {
            var message = new MailMessage();


            message.From = new MailAddress(Email.From.Email, Email.From.Name);

            message.To.Add(new MailAddress("Alahly.tri@gmail.com", "Biikaaaa"));

            Email.To.ForEach(x =>
            {
                message.To.Add(new MailAddress(x.Email, x.Name));
            });
            Email.Cc.ForEach(x =>
            {
                message.CC.Add(new MailAddress(x.Email, x.Name));
            });
            Email.Bcc.ForEach(x =>
            {
                message.Bcc.Add(new MailAddress(x.Email, x.Name));
            });
            message.Subject = Email.Subject;
            message.Body = Email.Content;
            message.IsBodyHtml = true;

            return message;
        }
        public bool SendEmailWithAttachment(Email Email)
        {
            try
            {
                var message = GetMessage(Email);
                if (Email.HasAttachments)
                    AddAttachmentTomessage(Email, message);
                SmtpClient client = GetClient();
                client.Send(message);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        private void AddAttachmentTomessage(Email email, MailMessage message)
        {
            email.Attachements.ForEach(x =>
            {
                message.Attachments.Add(new Attachment(x.OpenReadStream(), x.FileName));
            });
        }

    }
}
