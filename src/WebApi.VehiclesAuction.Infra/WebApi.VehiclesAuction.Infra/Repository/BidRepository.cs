using Microsoft.EntityFrameworkCore;
using System.Threading;
using WebApi.VehiclesAuction.Domain.Interfaces.Repository;
using WebApi.VehiclesAuction.Domain.Models.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<bool> UpdateWinnerStatus(Bid bid)
        {
            try
            {
                var existingEntity = _context.Set<Bid>().Find(bid.Id);

                if (existingEntity != null)
                {
                    existingEntity.Winner = true;
                    _context.Update(existingEntity);
                }
                else
                {
                    bid.Winner = true;
                }

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
