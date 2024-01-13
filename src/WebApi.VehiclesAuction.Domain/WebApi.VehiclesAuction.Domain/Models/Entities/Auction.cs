namespace WebApi.VehiclesAuction.Domain.Models.Entities
{
    public class Auction : Base
    {
        public Auction(string name, DateTime startAt, DateTime endAt, bool active)
        {
            Name = name;
            StartAt = new DateTime(startAt.Year, startAt.Month, startAt.Day, 0, 0, 0);
            EndAt = new DateTime(endAt.Year, endAt.Month, endAt.Day, 23, 59, 59);
            Active = active;
        }

        public Auction()
        {
            
        }

        public string Name { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<AuctionItem> Items { get; set; }
    }
}
