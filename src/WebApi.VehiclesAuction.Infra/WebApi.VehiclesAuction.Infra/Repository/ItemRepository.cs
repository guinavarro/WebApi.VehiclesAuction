using WebApi.VehiclesAuction.Domain.Interfaces.Repository;

namespace WebApi.VehiclesAuction.Infra.Repository
{
    public class ItemRepository : BaseRepository, IItemRepository
    {
        private readonly VehiclesAuctionContext _context;
        public ItemRepository(VehiclesAuctionContext context) : base(context)
        {
            _context = context;
        }
    }
}
