using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.VehiclesAuction.Domain.Models.Entities;

namespace WebApi.VehiclesAuction.Infra.DbMapping
{
    public class AuctionMap : BaseMap<Auction>
    {
        public AuctionMap() : base("AUCTION")
        { }

        public override void Configure(EntityTypeBuilder<Auction> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name).HasColumnName("NAME").HasColumnType("varchar(100)").IsRequired(true);
            builder.Property(x => x.StartAt).HasColumnName("START_AT").HasColumnType("timestamp without time zone").IsRequired(true);
            builder.Property(x => x.EndAt).HasColumnName("END_AT").HasColumnType("timestamp without time zone").IsRequired(true);
            builder.Property(x => x.Active).HasColumnName("ACTIVE").HasColumnType("boolean").HasDefaultValue("true").IsRequired(true);
        }
    }
}
