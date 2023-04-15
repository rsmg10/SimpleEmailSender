using EmailSender.Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EmailSender.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class EmailSender : ControllerBase
    {
        public EmailSender(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        // key = email
        // value = {"Subject":"<write your subject line>","Content":"<text or html content>","From":{"Name":"anyname","Email":"xxx@gmail.com"},"To":[{"Name":"anyname","Email":"xxx@yahoo.com"},{"Name":"anyname","Email":"xxx@gmail.com"}],"Cc":null,"Bcc":null}
        public IEmailSender _emailSender { get; set; }
        [HttpPost]
        public async Task<IActionResult> SendEmail(EmailVm? email)
        {
            try
            {
                var files = new List<IFormFile>();
                if (Request.ContentType != "application/json")
                {
                    var forms = await Request.ReadFormAsync();
                    files = forms.Files.ToList();
                    var content = forms.FirstOrDefault(k => k.Key == "email").Value;
                    email = JsonSerializer.Deserialize<EmailVm>(content);
                }else if (email is null) throw new Exception("no body available");
                return Ok(_emailSender.SendEmail(email.ToEmail(files)));
            }
            catch (Exception e)
            {

                throw;
            }
        }

    }
}