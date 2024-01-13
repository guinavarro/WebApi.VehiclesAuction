namespace WebApi.VehiclesAuction.Domain.Models.Models
{
    public class AuctionModel
    {
        public string AuctionName { get; set; }
        public Guid AuctionKey { get; set; }
        public string AuctionStartAt { get; set; }
        public string AuctionEndAt { get; set; }
        public bool AuctionIsActive { get; set; }
        public int TotalItems => Items.Count();
        public List<AuctionItemModel> Items { get; set; } = new();
    }
}
