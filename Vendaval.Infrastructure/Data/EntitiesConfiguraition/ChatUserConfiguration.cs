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
    public class ChatUserConfiguration : IEntityTypeConfiguration<ChatUser>
    {
        public void Configure(EntityTypeBuilder<ChatUser> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedAt).ValueGeneratedOnAdd().HasDefaultValueSql("NOW()");

            builder.Property(x => x.UpdatedAt).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("NOW()");

            builder.Property(x => x.ConnectionId).IsRequired();

            builder.Property(x => x.UserType).IsRequired();

            builder.Property(x => x.Email).IsRequired();

            builder.Property(x => x.Name).IsRequired();

            builder.Property(x => x.IsOnline).IsRequired();
        }
    }
}
