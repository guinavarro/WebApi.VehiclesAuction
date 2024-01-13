using Microsoft.Extensions.DependencyInjection;
using WebApi.VehiclesAuction.Domain.Interfaces.Repository;
using WebApi.VehiclesAuction.Domain.Interfaces.Services;
using WebApi.VehiclesAuction.Domain.Services;
using WebApi.VehiclesAuction.Infra.Repository;

namespace WebApi.VehiclesAuction.Infra
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<VehiclesAuctionContext>();

            services.AddScoped<IBaseRepository, BaseRepository>();
            services.AddScoped<IParticipantRepository, ParticipantRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IBidRepository, BidRepository>();
            services.AddScoped<IAuctionRepository, AuctionRepository>();
            services.AddScoped<IAuctionItemRepository, AuctionItemRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();

            services.AddScoped<IParticipantServices, ParticipantServices>();
            services.AddScoped<IAuctionServices, AuctionServices>();

            return services;
        }
    }
}
