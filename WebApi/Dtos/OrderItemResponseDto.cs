using Core.Entities.OrderShop;

namespace WebApi.Dtos
{
    public class OrderItemResponseDto
    {
        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}
