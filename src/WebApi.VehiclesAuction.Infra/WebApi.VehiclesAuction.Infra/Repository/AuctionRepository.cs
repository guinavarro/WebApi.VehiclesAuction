using WebApi.VehiclesAuction.Domain.Interfaces.Repository;

namespace WebApi.VehiclesAuction.Infra.Repository
{
    public class AuctionRepository : BaseRepository, IAuctionRepository
    {
        private readonly VehiclesAuctionContext _context;
        public AuctionRepository(VehiclesAuctionContext context) : base(context)
        {
            _context = context;
        }
    }
}
