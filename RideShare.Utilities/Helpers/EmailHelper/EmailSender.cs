using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RideShare.Utilities.Helpers.EmailHelper
{
   public  class EmailSender:IEmailSender
    {

        private readonly EmailOptions emailOptions;
        public EmailSender(IOptions<EmailOptions> options)
        {
            // congigure to get the values from appsetting.json
            emailOptions = options.Value;
        }


        public Task SendEmail(string email, string subject, string Message)
        {
            return Execute(emailOptions.SendGridKey, subject, Message, email);
        }

        private Task Execute(string sendGridKey, string subject, string message, string email)
        {
            var client = new SendGridClient(sendGridKey);
            var from = new EmailAddress("sachinshrestha483@gmail.com", "ABCD Company");
            var to = new EmailAddress(email, "Example User");
            //var plainTextContent = "and easy to do anywhere, even with C#";
            //var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var htmlContent = "<h3 style='text-align: center'  >" + "Your Email Verification Code is " + "</h3>"+


                "<br>"+

                "<p>"+


                "<h1 style='text-align: center'>"+message+"</h1>"


                + "</p>";
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, message, "");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);

            return client.SendEmailAsync(msg);
        }



    }
}
