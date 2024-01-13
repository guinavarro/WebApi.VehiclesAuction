using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.VehiclesAuction.Domain.Models.Entities;

namespace WebApi.VehiclesAuction.Infra.DbMapping
{
    public class AuctionItemMap : BaseMap<AuctionItem>
    {
        public AuctionItemMap() : base("AUCTION_ITEM")
        { }

        public override void Configure(EntityTypeBuilder<AuctionItem> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.AuctionId).HasColumnName("AUCTION_ID").HasColumnType("integer").IsRequired(true);
            builder.Property(x => x.ItemId).HasColumnName("ITEM_ID").HasColumnType("integer").IsRequired(true);

            builder.Property(x => x.MinimumBid).HasColumnName("MINIMUM_BID").HasColumnType("decimal(10,2)").IsRequired(false);
            builder.Property(x => x.StartAt).HasColumnName("START_AT").HasColumnType("timestamp without time zone").IsRequired(true);
            builder.Property(x => x.EndAt).HasColumnName("END_AT").HasColumnType("timestamp without time zone").IsRequired(true);

            builder.HasOne(x => x.Auction)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.AuctionId);

            builder.HasOne(x => x.Item)
             .WithOne(x => x.AuctionItem)
             .HasForeignKey<AuctionItem>(x => x.ItemId);

        }
    }
}
