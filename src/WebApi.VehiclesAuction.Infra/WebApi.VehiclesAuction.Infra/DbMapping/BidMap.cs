using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.VehiclesAuction.Domain.Models.Entities;

namespace WebApi.VehiclesAuction.Infra.DbMapping
{
    public class BidMap : BaseMap<Bid>
    {
        public BidMap() : base("BID")
        { }

        public override void Configure(EntityTypeBuilder<Bid> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.ParticipantId).HasColumnName("PARTICIPANT_ID").HasColumnType("integer").IsRequired(true);
            builder.Property(x => x.AuctionItemId).HasColumnName("AUCTION_ITEM_ID").HasColumnType("integer").IsRequired(true);
            builder.Property(x => x.Value).HasColumnName("VALUE").HasColumnType("decimal(10,2)").IsRequired(true);
            builder.Property(x => x.Winner).HasColumnName("WINNER").HasColumnType("boolean").IsRequired(true);

            builder.HasOne(x => x.Participant)
                .WithMany(x => x.Bids)
                .HasForeignKey(x => x.ParticipantId);

            builder.HasOne(x => x.AuctionItem)
                .WithMany(x => x.Bids)
                .HasForeignKey(x => x.AuctionItemId);





        }
    }
}
