using Core.Entities.OrderShop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Data.Configuration
{
    public class OrderShopConfiguration : IEntityTypeConfiguration<OrderShop>
    {
        public void Configure(EntityTypeBuilder<OrderShop> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, x =>
            {
                x.WithOwner();
            });

            builder.Property(s => s.Status)
                .HasConversion(
                o => o.ToString(),
                o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o)
            );

            builder.HasMany(o => o.OrderItems).WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(o => o.Subtotal).HasColumnType("decimal(18,2)");
        }
    }
}
