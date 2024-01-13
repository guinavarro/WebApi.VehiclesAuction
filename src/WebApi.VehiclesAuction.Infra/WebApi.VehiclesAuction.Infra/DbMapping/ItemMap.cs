using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.VehiclesAuction.Domain.Models.Entities;

namespace WebApi.VehiclesAuction.Infra.DbMapping
{
    public class ItemMap : BaseMap<Item>
    {
        public ItemMap() : base("ITEM")
        { }

        public override void Configure(EntityTypeBuilder<Item> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Name).HasColumnName("NAME").HasColumnType("VARCHAR(100)").IsRequired(true);
            builder.Property(x => x.Description).HasColumnName("DESCRIPTION").HasColumnType("VARCHAR(500)").IsRequired(false);
            builder.Property(x => x.Brand).HasColumnName("BRAND").HasColumnType("VARCHAR(100)").IsRequired(true);
            builder.Property(x => x.Type).HasColumnName("TYPE").HasColumnType("integer").IsRequired(true).HasConversion<int>();

        }
    }
}
