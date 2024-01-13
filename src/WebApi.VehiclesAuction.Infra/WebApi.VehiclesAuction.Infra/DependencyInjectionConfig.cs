using Microsoft.Extensions.DependencyInjection;

namespace WebApi.VehiclesAuction.Infra
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<VehiclesAuctionContext>();

            return services;
        }
    }
}
