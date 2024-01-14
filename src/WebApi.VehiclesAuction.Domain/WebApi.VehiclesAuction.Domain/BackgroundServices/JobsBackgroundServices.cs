using Hangfire.Server;
using WebApi.VehiclesAuction.Domain.Interfaces.Services;

namespace WebApi.VehiclesAuction.Domain.BackgroundServices
{
    public class JobsBackgroundServices
    {
        private readonly IAuctionServices _auctionServices;

        public JobsBackgroundServices(IAuctionServices auctionServices)
        {
            _auctionServices = auctionServices;
        }

        public async Task AuctionAudit(PerformContext? performContext) => await _auctionServices.DailyAuctionsAudit(performContext);
        public async Task NotifyWinners(PerformContext? performContext) => await _auctionServices.NotifyDailyWinners(performContext);
    }
}
