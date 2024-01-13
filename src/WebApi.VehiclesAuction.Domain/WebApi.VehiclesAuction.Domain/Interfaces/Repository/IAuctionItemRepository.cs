using WebApi.VehiclesAuction.Domain.Models.Entities;

namespace WebApi.VehiclesAuction.Domain.Interfaces.Repository
{
    public interface IAuctionItemRepository : IBaseRepository
    {
        Task<List<AuctionItem>> GetAuctionsItem(CancellationToken cancellationToken = default);
        Task<AuctionItem> GetAuctionItemByKey(Guid key, CancellationToken cancellationToken = default);
    }
}
