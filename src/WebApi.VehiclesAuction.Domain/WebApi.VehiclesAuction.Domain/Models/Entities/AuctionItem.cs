namespace WebApi.VehiclesAuction.Domain.Models.Entities
{
    public class AuctionItem : Base
    {
        public int AuctionId { get; set; }
        public int ItemId { get; set; }
        public decimal? MinimumBid { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public virtual Auction Auction { get; set; }
        public virtual Item Item { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
    }
}
