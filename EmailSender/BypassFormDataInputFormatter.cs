using Microsoft.AspNetCore.Mvc.Formatters;

namespace EmailSender
{
    /// <summary>
    /// this is used to allow both form rrequests and json requests to be accepted, 
    /// so we can send emails with attachements or without with different request types
    /// send email with no attachment using json
    /// send email with attacment using forms
    /// </summary>
    public class BypassFormDataInputFormatter : IInputFormatter
    {
        public bool CanRead(InputFormatterContext context)
        {
            return context.HttpContext.Request.HasFormContentType;
        }

        public Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
        {
            return InputFormatterResult.SuccessAsync(null);
        }
    }
}