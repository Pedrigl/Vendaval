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
    public class ProductAvaliationConfiguration : IEntityTypeConfiguration<ProductAvaliation>
    {
        public void Configure(EntityTypeBuilder<ProductAvaliation> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.ProductId).IsRequired();

            builder.Property(x => x.CostumerName).IsRequired().HasMaxLength(100);

            builder.Property(x => x.Title).IsRequired().HasMaxLength(100);

            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);

            builder.Property(x => x.Stars).IsRequired();
        }
    }
}
