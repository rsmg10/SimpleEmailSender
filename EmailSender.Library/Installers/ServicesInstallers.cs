using EmailSender.Library.Models;
using EmailSender.Library.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.Library.Installers
{
    public static class Installers
    {
        public static IServiceCollection ConfigureEmailSender(this IServiceCollection services, Version version = Version.MailKit)
        {
            services.EmailSenderServices(version);
            services.EmailSenderOptions();

            return services;
        }

        private static IServiceCollection EmailSenderOptions(this IServiceCollection services)
        {

            using (var scope = services.BuildServiceProvider())
            {

                var config = scope.GetRequiredService<IConfiguration>();
                var options = config.GetSection("EmailSenderOptions");
                var sendingEmail = options.Get<EmailSenderOptions>();

                var test = Environment.GetEnvironmentVariable("Port");
                Console.WriteLine(test);


                if (!options.Exists())
                    services.Configure<EmailSenderOptions>(o =>
                       {
                           o.Email = Environment.GetEnvironmentVariable("Email");
                           o.AppPassword = Environment.GetEnvironmentVariable("AppPassword");
                           o.Port = int.Parse(Environment.GetEnvironmentVariable("Port").Replace('"',' ') ?? "0") ;
                           o.EmailDomain = Environment.GetEnvironmentVariable("EmailDomain");
                           o.AllowedContentType = Environment.GetEnvironmentVariable("AllowedContentType");
                       });
                else
                    services.Configure<EmailSenderOptions>(options);
            };

            return services;
        }
        private static IServiceCollection EmailSenderServices(this IServiceCollection services, Version version)
        {
            switch (version)
            {
                case Version.MailKit:
                    services.AddTransient<IEmailSender, EmailSender_MailKit>();
                    break;
                case Version.NetMail:
                    services.AddTransient<IEmailSender, EmailSender_NetMail>();
                    break;
                default:
                    break;
            }
            return services;
        }
    }
    public enum Version
    {
        MailKit = 1, NetMail = 2,
    }
}
