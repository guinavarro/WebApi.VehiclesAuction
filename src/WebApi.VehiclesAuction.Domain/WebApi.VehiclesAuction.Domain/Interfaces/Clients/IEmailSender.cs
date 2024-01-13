namespace WebApi.VehiclesAuction.Domain.Interfaces.Clients
{
    public interface IEmailSender
    {
        Task SendEmail(string subject, string toEmail, string toUsername, string message);
    }
}
