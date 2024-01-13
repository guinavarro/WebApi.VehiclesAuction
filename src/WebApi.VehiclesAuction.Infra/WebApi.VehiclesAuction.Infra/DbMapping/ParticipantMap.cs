using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.VehiclesAuction.Domain.Models.Entities;

namespace WebApi.VehiclesAuction.Infra.DbMapping
{
    public class ParticipantMap : BaseMap<Participant>
    {
        public ParticipantMap() : base("PARTICIPANT")
        { }

        public override void Configure(EntityTypeBuilder<Participant> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name).HasColumnName("NAME").HasColumnType("varchar(100)").IsRequired(true);
            builder.Property(x => x.Email).HasColumnName("EMAIL").HasColumnType("varchar(100)").IsRequired(true);
            builder.Property(x => x.AddressId).HasColumnName("ADDRESS_ID").HasColumnType("integer").IsRequired(true);

            builder.HasOne(x => x.Address)
                .WithOne(x => x.Participant)
                .HasForeignKey<Participant>(x => x.AddressId);

            builder.HasMany(x => x.Bids)
                .WithOne(x => x.Participant)
                .HasForeignKey(x => x.ParticipantId);
        }
    }
}
