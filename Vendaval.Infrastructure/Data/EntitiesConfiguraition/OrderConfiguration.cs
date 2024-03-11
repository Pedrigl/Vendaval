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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("NOW()");

            builder.Property(x => x.UpdatedAt).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("NOW()");

            builder.Property(x => x.CustomerId).IsRequired();

            builder.HasMany(x => x.Products).WithMany();

            builder.Property(x => x.Total).IsRequired();

            builder.Property(x => x.Status).IsRequired();

            builder.Property(x => x.PaymentMethod).IsRequired();

            builder.Property(x => x.Installments).IsRequired();

            builder.Property(x => x.InstallmentValue).IsRequired();

            builder.Property(x => x.PaymentDate).IsRequired(false);

            builder.OwnsOne(x => x.DeliveryAddress);

            builder.Property(x => x.DeliveryType).IsRequired();

            builder.Property(x => x.TrackingCode).IsRequired(false);

            builder.Property(x => x.OrderNotes).IsRequired(false);

        }
    }
}
