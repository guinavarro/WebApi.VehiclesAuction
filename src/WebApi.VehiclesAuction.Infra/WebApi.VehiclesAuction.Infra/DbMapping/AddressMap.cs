using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.VehiclesAuction.Domain.Models.Entities;

namespace WebApi.VehiclesAuction.Infra.DbMapping
{
    public class AddressMap : BaseMap<Address>
    {
        public AddressMap() : base("ADDRESS")
        { }

        public override void Configure(EntityTypeBuilder<Address> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Street).HasColumnName("STREET").HasColumnType("varchar(100)").IsRequired(true);
            builder.Property(x => x.Number).HasColumnName("NUMBER").HasColumnType("varchar(10)").IsRequired(false);
            builder.Property(x => x.District).HasColumnName("DISTRICT").HasColumnType("varchar(100)").IsRequired(true);
            builder.Property(x => x.Cep).HasColumnName("CEP").HasColumnType("varchar(8)").IsRequired(true);
            builder.Property(x => x.City).HasColumnName("CITY").HasColumnType("varchar(100)").IsRequired(true);

      
        }
    }
}
