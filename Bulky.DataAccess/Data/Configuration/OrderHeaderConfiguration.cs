using Bulky.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulky.DataAccess.Data.Configuration;
public class OrderHeaderConfiguration : IEntityTypeConfiguration<OrderHeader>
{
    public void Configure(EntityTypeBuilder<OrderHeader> builder)
    {
        builder.HasMany<OrderDetail>()
            .WithOne(o => o.OrderHeader)
            .HasForeignKey(o => o.OrderHeaderId)
            .IsRequired(true);
    }
}
