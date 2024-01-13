using WebApi.VehiclesAuction.Domain.Interfaces.Repository;

namespace WebApi.VehiclesAuction.Infra.Repository
{
    public class AuctionItemRepository : BaseRepository, IAuctionItemRepository
    {
        private readonly VehiclesAuctionContext _context;
        public AuctionItemRepository(VehiclesAuctionContext context) : base(context)
        {
            _context = context;
        }
    }
}
