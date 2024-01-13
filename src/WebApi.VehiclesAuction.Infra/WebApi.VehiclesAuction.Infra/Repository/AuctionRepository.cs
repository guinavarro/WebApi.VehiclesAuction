using Microsoft.EntityFrameworkCore;
using WebApi.VehiclesAuction.Domain.Interfaces.Repository;
using WebApi.VehiclesAuction.Domain.Models.Entities;

namespace WebApi.VehiclesAuction.Infra.Repository
{
    public class AuctionRepository : BaseRepository, IAuctionRepository
    {
        private readonly VehiclesAuctionContext _context;
        public AuctionRepository(VehiclesAuctionContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ICollection<Auction>> GetAll(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Auction.Include(x => x.Items).ThenInclude(x => x.Item).AsNoTracking().ToListAsync(cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Auction> GetAuctionByKey(Guid key, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Auction
                    .Include(x => x.Items)
                    .ThenInclude(x => x.Item)
                    .FirstOrDefaultAsync(x => x.Key == key, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
