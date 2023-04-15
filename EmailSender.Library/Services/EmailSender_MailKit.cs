using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using EmailSender.Library.Models;

namespace EmailSender.Library.Services
{
    public class EmailSender_MailKit : IEmailSender
    {
        private readonly EmailSenderOptions _options;
        public EmailSender_MailKit(EmailSenderOptions options) => _options = options;
        public EmailSender_MailKit(IOptions<EmailSenderOptions> options) => _options = options.Value;

        //public bool SendEmail(Email Email)
        //{
        //    try
        //    {
        //        //Environment.SetEnvironmentVariable("mypass", "ewfahttqlsqectrv");
        //        //var pass = Environment.GetEnvironmentVariable("mypass");
        //        var message = GetMessage(Email);
        //        Send(message);
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //}

        public bool SendEmail(Email Email)
        {
            try
            {
                var message = GetMessage(Email);

                var attachment = new MimePart();
                Multipart body = new Multipart(Email.HasAttachments ? "mixed" : "plain") { message.Body };
                if (Email.HasAttachments)
                {
                    if (Email.Attachements.Any(file => !_options.AllowedContentType.Contains(file.ContentType))) return false;
                    var attachments = Email.Attachements.Select(a => a.OpenReadStream()).ToList();
                    var i = 0;
                    attachments.ForEach(a =>
                    {
                        attachment = new MimePart
                        {
                            Content = new MimeContent(a),
                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                            ContentTransferEncoding = ContentEncoding.Base64,
                            FileName = Email.Attachements[i].FileName,
                            IsAttachment = Email.HasAttachments
                        };
                        i++;
                        body.Add(attachment);
                    }
                    );
                }
                message.Body = body;

                Send(message);
                return true;
            }
            catch (Exception e)
            {
                return false;
                throw;
            }
        }
        private void Send(MimeMessage message)
        {
            using var emailClient = new SmtpClient();

            if (_options != null)
            {
                if (_options.EmailDomain == "smtp.gmail.com")
                    emailClient.Connect(_options.EmailDomain, _options.Port,
                    true);
                else
                    emailClient.Connect(_options.EmailDomain, _options.Port,
                      MailKit.Security.SecureSocketOptions.Auto);

                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(_options.Email, _options.AppPassword);
                emailClient.Send(message);
                emailClient.Disconnect(true);
            }
        }
        private static MimeMessage GetMessage(Email Email)
        {
            var message = new MimeMessage();
            var fromEmail = MailboxAddress.Parse(Email.From.Email);
            fromEmail.Name = Email.From.Name;
            message.From.Add(fromEmail);

            Email.To.ForEach(x =>
            {
                var t = MailboxAddress.Parse(x.Email);
                t.Name = x.Name;
                message.To.Add(t);
            });
            Email.Cc?.ForEach(x =>
            {
                var t = MailboxAddress.Parse(x.Email);
                t.Name = x.Name;
                message.Cc.Add(t);
            });
            Email.Bcc?.ForEach(x =>
            {
                var t = MailboxAddress.Parse(x.Email);
                t.Name = x.Name;
                message.Bcc.Add(t);
            });
            message.Subject = Email.Subject;


            var body = new TextPart("plain")
            {
                Text = Email.Content
            };
            message.Body = body;
            //message.HtmlBody= true;
            return message;
        }
    }
}
