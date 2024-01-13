using WebApi.VehiclesAuction.Domain.Interfaces.Repository;

namespace WebApi.VehiclesAuction.Infra.Repository
{
    public class BidRepository : BaseRepository, IBidRepository
    {
        private readonly VehiclesAuctionContext _context;
        public BidRepository(VehiclesAuctionContext context) : base(context)
        {
            _context = context;
        }
    }
}
