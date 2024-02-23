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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            
            builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
            
            builder.Property(x => x.Password).IsRequired().HasMaxLength(100);

            builder.Property(x => x.PhoneNumber).HasMaxLength(20);

            builder.OwnsMany(x => x.Address);

            builder.Property(x => x.BirthDate).IsRequired();
        }
    }
}
