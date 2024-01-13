using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WebApi.VehiclesAuction.Domain.Models.Entities;
using WebApi.VehiclesAuction.Infra.DbMapping;

namespace WebApi.VehiclesAuction.Infra
{
    public class VehiclesAuctionContext : IdentityDbContext
    {
        public VehiclesAuctionContext(DbContextOptions<VehiclesAuctionContext> options) : base(options)
        {
            // Configurações para lidar com as colunas de date no pg
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
        }

        #region DbSets
        public DbSet<Address> Address { get; set; }
        public DbSet<Auction> Auction { get; set; }
        public DbSet<AuctionItem> AuctionItem { get; set; }
        public DbSet<Bid> Bid { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Participant> Participant { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasPostgresExtension("uuid-ossp");
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            builder.ApplyConfiguration(new AddressMap());
            builder.ApplyConfiguration(new AuctionItemMap());
            builder.ApplyConfiguration(new AuctionMap());
            builder.ApplyConfiguration(new BidMap());
            builder.ApplyConfiguration(new ParticipantMap());
        }
    }
}
