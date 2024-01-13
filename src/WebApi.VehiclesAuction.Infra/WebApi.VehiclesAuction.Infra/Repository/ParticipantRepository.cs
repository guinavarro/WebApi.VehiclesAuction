using WebApi.VehiclesAuction.Domain.Interfaces.Repository;

namespace WebApi.VehiclesAuction.Infra.Repository
{
    public class ParticipantRepository : BaseRepository, IParticipantRepository
    {
        private readonly VehiclesAuctionContext _context;
        public ParticipantRepository(VehiclesAuctionContext context) : base(context)
        {
            _context = context;
        }
    }
}
