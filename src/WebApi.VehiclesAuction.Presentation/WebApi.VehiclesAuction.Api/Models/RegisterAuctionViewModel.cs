namespace WebApi.VehiclesAuction.Api.Models
{
    public class RegisterAuctionViewModel
    {
        public string Name { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public bool IsActive { get; set; }
    }
}
