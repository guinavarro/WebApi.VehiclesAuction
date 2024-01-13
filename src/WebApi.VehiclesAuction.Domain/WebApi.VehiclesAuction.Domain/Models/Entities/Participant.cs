namespace WebApi.VehiclesAuction.Domain.Models.Entities
{
    public class Participant : Base
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int AddressId { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
        public virtual Address Address { get; set; }
    }
}
