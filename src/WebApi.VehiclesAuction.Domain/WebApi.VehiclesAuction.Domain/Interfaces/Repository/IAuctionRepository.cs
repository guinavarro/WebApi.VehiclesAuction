using WebApi.VehiclesAuction.Domain.Models.Entities;

namespace WebApi.VehiclesAuction.Domain.Interfaces.Repository
{
    public interface IAuctionRepository : IBaseRepository
    {
        Task<ICollection<Auction>> GetAll(CancellationToken cancellationToken = default);
        Task<Auction> GetAuctionByKey(Guid key, CancellationToken cancellationToken = default);
    }
}
