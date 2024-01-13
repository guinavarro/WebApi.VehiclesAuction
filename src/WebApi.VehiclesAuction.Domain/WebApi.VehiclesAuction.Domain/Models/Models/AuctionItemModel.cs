namespace WebApi.VehiclesAuction.Domain.Models.Models
{
    public class AuctionItemModel
    {
        public Guid? AuctionItemKey { get; set; }
        public string AuctionItemName { get; set; }
        public string AuctionItemDescription { get; set; }
        public string AuctionItemBrand { get; set; }
        public int AuctionItemType { get; set; }
        public decimal? AuctionItemMinimumBid { get; set; }
        public TimeSpan AuctionItemStartAtHours { get; set; }
        public TimeSpan AuctionItemEndAtHours { get; set; }
    }
}
