namespace WebApi.VehiclesAuction.Domain.Models.Entities
{
    public abstract class Base
    {
        public int Id { get; set; }
        public Guid Key { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set;}
    }
}
