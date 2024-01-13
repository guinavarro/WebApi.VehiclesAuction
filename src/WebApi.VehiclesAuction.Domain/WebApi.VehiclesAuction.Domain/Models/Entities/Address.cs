using WebApi.VehiclesAuction.Domain.Util;

namespace WebApi.VehiclesAuction.Domain.Models.Entities
{
    public class Address : Base
    {
        public Address(string street, string number, string district, string cep, string city)
        {
            Street = street;
            Number = number;
            District = district;
            Cep = cep.OnlyDigits();
            City = city;
        }

        public Address()
        {
            
        }

        public string Street { get; set; }
        public string Number { get; set; }
        public string District { get; set; }
        public string Cep { get; set; }
        public string City { get; set; }
        public Participant Participant { get; set; }

    }
}
