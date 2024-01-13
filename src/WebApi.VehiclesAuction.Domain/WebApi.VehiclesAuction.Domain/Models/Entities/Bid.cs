namespace WebApi.VehiclesAuction.Domain.Models.Entities
{
    public class Bid : Base
    {
        public int ParticipantId { get; set; }
        public int AuctionItemId { get; set; }
        public decimal Value { get; set; }
        public bool Winner { get; set; }
        public virtual Participant Participant { get; set; }
        public virtual AuctionItem AuctionItem { get; set; }
    }
}
