using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Entities;

namespace Vendaval.Infrastructure.Data.EntitiesConfiguraition
{
    public class AddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd();
            builder.Property(a => a.UserId).IsRequired();
            builder.Property(a => a.Street).IsRequired().HasMaxLength(100);
            builder.Property(a => a.Number).IsRequired().HasMaxLength(10);
            builder.Property(a => a.Complement).HasMaxLength(100);
            builder.Property(a => a.Neighborhood).IsRequired().HasMaxLength(100);
            builder.Property(a => a.City).IsRequired().HasMaxLength(100);
            builder.Property(a => a.State).IsRequired().HasMaxLength(100);
            builder.Property(a => a.Country).IsRequired().HasMaxLength(100);
            builder.Property(a => a.ZipCode).IsRequired().HasMaxLength(10);
        }
    }
}
