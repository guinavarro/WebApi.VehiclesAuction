using WebApi.VehiclesAuction.Domain.Models.Enums;

namespace WebApi.VehiclesAuction.Domain.Models.Entities
{
    public class Item : Base
    {
        public Item(string name, string description, string brand, VehicleType type)
        {
            Name = name;
            Description = description;
            Brand = brand;
            Type = type;
        }

        public Item()
        {
            
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public VehicleType Type { get; set; }
        public virtual AuctionItem AuctionItem { get; set; }
    }
}
