namespace WebApi.VehiclesAuction.Api.Models
{
    public class RegisterAuctionItemsViewModel
    {
        public Guid AuctionKey { get; set; }
        public List<AuctionItemsToRegister> AuctionItems { get; set; }
    }

    public class AuctionItemsToRegister
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public int Type { get; set; }
        public decimal? MinimumBid { get; set; }
        public int StartHours { get; set; }
        public int StartMinutes { get; set; }
        public int EndHours { get; set; }
        public int EndMinutes { get; set; }
    }

}
