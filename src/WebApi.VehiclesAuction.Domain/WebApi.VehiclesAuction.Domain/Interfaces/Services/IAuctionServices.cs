using WebApi.VehiclesAuction.Domain.Models;
using WebApi.VehiclesAuction.Domain.Models.Entities;
using WebApi.VehiclesAuction.Domain.Models.Models;

namespace WebApi.VehiclesAuction.Domain.Interfaces.Services
{
    public interface IAuctionServices
    {
        Task<Response<bool>> RegisterAuction(string name, DateTime startAt, DateTime endAt, bool isActive, CancellationToken cancellationToken = default);
        Task<Response<List<AuctionModel>>> GetAllAuctions(CancellationToken cancellationToken = default);
        Task<Response<bool>> UpdateAuctionStatus(Guid auctionKey, bool newActiveStatus, CancellationToken cancellationToken = default);
        Task<Response<AuctionModel>> GetAuctionByKey(Guid auctionKey, CancellationToken cancellationToken = default);
        Task<Response<bool>> RegisterAuctionItems(Guid auctionKey, List<AuctionItemModel> items, CancellationToken cancellationToken = default);
        Task<Response<bool>> BidItemAuction(Guid itemAuctionKey, decimal bidValue, int participantId, CancellationToken cancellationToken = default);
        Task<Response<bool>> RemoveAuctionItem(Guid itemAuctionKey, CancellationToken cancellationToken = default);
        Task<Response<bool>> RemoveAuction(Guid auctionKey, CancellationToken cancellationToken = default);
        Task DailyAuctionsAudit();
        Task NotifyDailyWinners();
    }
}
