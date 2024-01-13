using Microsoft.EntityFrameworkCore;
using WebApi.VehiclesAuction.Domain.Interfaces.Repository;
using WebApi.VehiclesAuction.Domain.Models.Entities;

namespace WebApi.VehiclesAuction.Infra.Repository
{
    public class AuctionItemRepository : BaseRepository, IAuctionItemRepository
    {
        private readonly VehiclesAuctionContext _context;
        public AuctionItemRepository(VehiclesAuctionContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<AuctionItem>> GetAuctionsItem(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.AuctionItem
                    .Include(x => x.Auction)
                    .Include(x => x.Item)
                    .Include(x => x.Bids)
                        .ThenInclude(x => x.Participant)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AuctionItem> GetAuctionItemByKey(Guid key, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.AuctionItem
                    .Include(x => x.Auction)
                    .Include(x => x.Item)
                    .Include(x => x.Bids)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Key == key, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
