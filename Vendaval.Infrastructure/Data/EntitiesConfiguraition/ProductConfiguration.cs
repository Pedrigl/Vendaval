using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vendaval.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Vendaval.Infrastructure.Data.EntitiesConfiguraition
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("NOW()");

            builder.Property(x => x.UpdatedAt).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("NOW()");

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);

            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);

            builder.Property(x => x.Price).IsRequired();

            builder.Property(x => x.Stock).IsRequired();

            builder.Property(x => x.Image).IsRequired().HasMaxLength(500);

            builder.Property(x => x.Category).IsRequired();

            builder.Property(x => x.Avaliation);
        }
    }
}
