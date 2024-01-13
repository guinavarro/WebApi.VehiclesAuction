namespace WebApi.VehiclesAuction.Domain.Models.Entities
{
    public class Address : Base
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string District { get; set; }
        public string Cep { get; set; }
        public string City { get; set; }
        public Participant Participant { get; set; }
    }
}
