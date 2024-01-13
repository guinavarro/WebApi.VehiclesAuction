namespace WebApi.VehiclesAuction.Domain.Models.Entities
{
    public class Participant : Base
    {
        public Participant(string name, string email, int addressId)
        {
            Name = name;
            Email = email;
            AddressId = addressId;
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public int AddressId { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }
        public virtual Address Address { get; set; }
    }
}
