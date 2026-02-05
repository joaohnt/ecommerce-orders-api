using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Database.Mapping;

public class OrderMap : IEntityTypeConfiguration<Order> 
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        
        builder.Property(o => o.Id).ValueGeneratedOnAdd().UseIdentityColumn();
        
        builder.Property(o => o.CreatedAt).HasColumnName("CreatedAt").HasDefaultValueSql("getdate()").IsRequired();
        builder.Property(o => o.UpdatedAt).HasColumnName("UpdatedAt");
        builder.Property(o => o.Status).HasColumnName("Status").IsRequired();
        
        builder.HasMany(o => o.OrderItems).WithOne(o => o.Order).HasForeignKey(o => o.OrderId).OnDelete(DeleteBehavior.Cascade).IsRequired();
        
    }
}