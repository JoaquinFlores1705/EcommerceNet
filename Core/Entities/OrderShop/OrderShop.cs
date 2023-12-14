using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrderShop
{
    public class OrderShop : BaseClass
    {
        public OrderShop()
        {
            
        }
        public OrderShop(string buyerEmail, Direction shippingAddress, ShippingType shippingType, IReadOnlyList<OrderItem> orderItems, decimal subtotal)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            ShippingType = shippingType;
            OrderItems = orderItems;
            Subtotal = subtotal;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderShopDate { get; set; } = DateTimeOffset.Now;
        public Direction ShippingAddress { get; set; }
        public ShippingType ShippingType { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string PaymentAttemptId { get; set; }
        public decimal GetTotal() {
            return Subtotal + ShippingType.Price;
        }
    }
}
