using Microsoft.EntityFrameworkCore;
using System.Threading;
using WebApi.VehiclesAuction.Domain.Interfaces.Repository;
using WebApi.VehiclesAuction.Domain.Models.Entities;

namespace WebApi.VehiclesAuction.Infra.Repository
{
    public class BidRepository : BaseRepository, IBidRepository
    {
        private readonly VehiclesAuctionContext _context;
        public BidRepository(VehiclesAuctionContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Bid>> GetBidWinnersByDate(DateTime date, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Bid
                    .Include(x => x.Participant)
                    .Include(x => x.AuctionItem)
                        .ThenInclude(x => x.Item)
                    .Where(x => x.Winner == true && x.CreatedAt.Date == date.Date)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
