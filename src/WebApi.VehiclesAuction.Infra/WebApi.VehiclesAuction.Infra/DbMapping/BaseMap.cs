using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.VehiclesAuction.Domain.Models.Entities;

namespace WebApi.VehiclesAuction.Infra.DbMapping
{
    public class BaseMap<T> : IEntityTypeConfiguration<T> where T: Base
    {
        private readonly string _tableName;

        public BaseMap(string tableName)
        {
            _tableName = tableName;
        }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            if (!string.IsNullOrEmpty(_tableName))
                builder.ToTable(_tableName);

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd();
            builder.Property(_ => _.Key).HasDefaultValueSql("uuid_generate_v4()");
            builder.Property(_ => _.CreatedAt).HasColumnType("timestamp without time zone")
                                                .HasDefaultValueSql("NOW()")
                                                .ValueGeneratedOnAdd();
            builder.Property(_ => _.UpdatedAt).HasColumnType("timestamp without time zone")
                                                .ValueGeneratedOnUpdate()
                                                .IsRequired(false);

        }
    }
}
