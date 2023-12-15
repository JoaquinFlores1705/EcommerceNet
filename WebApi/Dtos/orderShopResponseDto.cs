using Core.Entities.OrderShop;
using System.Collections.Generic;
using System;

namespace WebApi.Dtos
{
    public class orderShopResponseDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderShopDate { get; set; }
        public Direction ShippingAddress { get; set; }
        public string ShippingType { get; set; }
        public decimal ShippingTypePrice { get; set; }
        public IReadOnlyList<OrderItemResponseDto> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
    }
}
