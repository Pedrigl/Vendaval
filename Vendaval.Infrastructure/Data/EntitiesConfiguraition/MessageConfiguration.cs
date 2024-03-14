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
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("NOW()");

            builder.Property(x => x.UpdatedAt).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("NOW()");

            builder.Property(x => x.Content).IsRequired();

            builder.Property(x => x.SenderId).IsRequired();

            builder.Property(x => x.ReceiverId).IsRequired();

            builder.OwnsMany(x => x.Media);

            builder.HasAlternateKey(x => x.ConversationId);
        }
    }
}
