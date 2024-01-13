using SendGrid;
using SendGrid.Helpers.Mail;
using WebApi.VehiclesAuction.Domain.Interfaces.Clients;

namespace WebApi.VehiclesAuction.Domain.Clients
{
    public class EmailSender : IEmailSender
    {
        public EmailSender()
        {
            
        }
        public async Task SendEmail(string subject, string toEmail, string toUsername, string message)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("gusnavarro66@gmail.com", "Vehicle Auction API");
            var to = new EmailAddress(toEmail, toUsername);
            var plainTextContent = message;
            var htmlContent = string.Empty;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            await client.SendEmailAsync(msg);
        }
      
    }
}
