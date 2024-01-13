using WebApi.VehiclesAuction.Domain.Interfaces.Repository;

namespace WebApi.VehiclesAuction.Infra.Repository
{
    public class AddressRepository : BaseRepository, IAddressRepository
    {
        private readonly VehiclesAuctionContext _context;
        public AddressRepository(VehiclesAuctionContext context) : base(context)
        {
            _context = context;
        }
    }
}
