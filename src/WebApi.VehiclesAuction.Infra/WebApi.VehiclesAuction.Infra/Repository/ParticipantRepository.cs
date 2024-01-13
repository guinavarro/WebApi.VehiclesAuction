using Microsoft.EntityFrameworkCore;
using WebApi.VehiclesAuction.Domain.Interfaces.Repository;
using WebApi.VehiclesAuction.Domain.Models.Entities;

namespace WebApi.VehiclesAuction.Infra.Repository
{
    public class ParticipantRepository : BaseRepository, IParticipantRepository
    {
        private readonly VehiclesAuctionContext _context;
        public ParticipantRepository(VehiclesAuctionContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Participant> GetParticipantByEmail(string email, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Participant
                    .Include(x => x.Address)
                    .Include(x => x.Bids)
                        .ThenInclude(x => x.AuctionItem)
                    .Where(x => x.Email == email)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Participant> GetParticipantById(int id, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Participant
                    .Include(x => x.Address)
                    .Include(x => x.Bids)
                        .ThenInclude(x => x.AuctionItem)
                    .Where(x => x.Id == id)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
