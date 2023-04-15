using Microsoft.AspNetCore.Http;
using System.Security.Principal;

namespace EmailSender.Library.Models
{
    public interface IEmailSender
    {
        public bool SendEmail(Email Email);
    }

    public class EmailSenderOptions
    {
        public string EmailDomain { get; set; }
        public string Email { get; set; }
        public string AppPassword { get; set; }
        public int Port { get; set; }
        public string AllowedContentType { get; set; } // comma-seperated content types
    }
    public class EmailVm
    {
        public string Subject { get; set; }
        public string Content { get; set; }
        public EmailPaticipant From { get; set; }
        public List<EmailPaticipant> To { get; set; }
        public List<EmailPaticipant>? Cc { get; set; }
        public List<EmailPaticipant>? Bcc { get; set; }
        public Email ToEmail(List<IFormFile> files)
        {
            var email = new Email()
            {
                Bcc = Bcc,
                Subject = Subject,
                Content = Content,
                From = From,
                To = To,
                Cc = Cc,
                Attachements = new List<EmailAttachment>()
            };
            if (files.Any()) files.ForEach(f=> email.Attachements.Add(new EmailAttachment(f.FileName, f.OpenReadStream(), f.ContentType)));

            return email;


        }
    }
    public class Email
    {
        public EmailPaticipant From { get; set; }
        public List<EmailPaticipant> To { get; set; }
        public List<EmailPaticipant> Cc { get; set; }
        public List<EmailPaticipant> Bcc { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool HasAttachments { get; set; } = false;

        private List<EmailAttachment> _attachements;
        public List<EmailAttachment> Attachements
        {
            get => _attachements;
            set
            {
                _attachements = value;
                HasAttachments = true;
            }
        }
    }
    public record EmailAttachment (string Name, Stream File, string ContentType);

    public record EmailPaticipant(string Name, string Email);
}