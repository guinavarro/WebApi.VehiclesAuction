namespace WebApi.VehiclesAuction.Domain.Models.Entities
{
    public class AuctionItem : Base
    {
        public AuctionItem(int auctionId, int itemId, decimal? minimumBid, TimeSpan startAt, TimeSpan endAt)
        {
            AuctionId = auctionId;
            ItemId = itemId;
            MinimumBid = minimumBid;
            StartAtHours = startAt;
            EndAtHours = endAt;
            CurrentValue = minimumBid;
        }
        public AuctionItem()
        {
            
        }

        public int AuctionId { get; set; }
        public int ItemId { get; set; }
        public decimal? MinimumBid { get; set; }
        public decimal? CurrentValue { get; set; }
        public TimeSpan StartAtHours { get; set; }
        public TimeSpan EndAtHours { get; set; }
        public virtual Auction Auction { get; set; }
        public virtual Item Item { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }

        public void UpdateCurrentValue(decimal value) => CurrentValue = value;
    }
}
