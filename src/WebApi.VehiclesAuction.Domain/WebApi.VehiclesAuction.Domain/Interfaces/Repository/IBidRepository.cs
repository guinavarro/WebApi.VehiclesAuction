using WebApi.VehiclesAuction.Domain.Models.Entities;

namespace WebApi.VehiclesAuction.Domain.Interfaces.Repository
{
    public interface IBidRepository : IBaseRepository
    {
        Task<List<Bid>> GetBidWinnersByDate(DateTime date, CancellationToken cancellationToken = default);
    }
}
