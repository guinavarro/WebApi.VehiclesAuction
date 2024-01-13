namespace WebApi.VehiclesAuction.Domain.Models.Entities
{
    public class Auction : Base
    {
        public string Name { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<AuctionItem> Items { get; set; }
    }
}
