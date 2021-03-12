using Microsoft.Extensions.Configuration;
using ServiceRequests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ServiceRequests.Services
{
    public class EmailServices : IEmailServices
    {
        IConfiguration _configuration;
        public EmailServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private readonly object email_locker = new object();
        /// <summary>
        /// Function to send emails
        /// Since it will be called from different threads, lock is necessary
        /// </summary>
        /// <param name="serviceModel"></param>
        public void SendEmail(ServiceModel serviceModel)
        {
            lock (email_locker) {
                var _message = new MailMessage();

                _message.From = new MailAddress(_configuration.GetValue<string>("SmtpSettings:Email"));

                _message.Subject = $"Service GUID {serviceModel.Id} has changed status to {serviceModel.CurrentStatus}";
                // Assuming we have a list of addresses, emails and names
                _message.To.Add(LookForCreatorEmail(serviceModel.CreatedBy));
                _message.Body = $"<h1> <<< message withh all service model information goes here>>> </h1>";

                _message.IsBodyHtml = true;

                try
                {
                    using (var smtp = new SmtpClient())
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Host = _configuration.GetValue<string>("SmtpSettings:Host");
                        //"smtp-mail.outlook.com";
                        smtp.Port = _configuration.GetValue<int>("SmtpSettings:Port");
                        smtp.Credentials = new System.Net.NetworkCredential(
                             _configuration.GetValue<string>("SmtpSettings:Email"),
                             _configuration.GetValue<string>("SmtpSettings:Password"));
                        smtp.EnableSsl = true;
                        smtp.Send(_message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }
            }
        }
        private MailAddress LookForCreatorEmail(string CreatedBy)
        {
            //TODO
#if DEBUG
            return new MailAddress("nico_k30@hotmail.com");
#else
            return new MailAddress("<founded email goes here>");
#endif
        }
    }
}
