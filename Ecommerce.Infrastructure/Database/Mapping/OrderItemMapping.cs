using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Database.Mapping;

public class OrderItemMap : IEntityTypeConfiguration<OrderItem> 
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItem");
        
        builder.Property(o => o.Id).ValueGeneratedOnAdd().UseIdentityColumn();
        
        builder.Property(o  => o.Name).HasMaxLength(100).IsRequired();
        builder.Property(o => o.Price).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(o => o.Quantity).HasColumnType("int").IsRequired();
        
    }
}