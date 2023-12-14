using Core.Entities.OrderShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IOrderShopService
    {
        Task<OrderShop> AddOrderShopAsync(string buyerEmail, int shippingType, string cartId, Direction direction);
        Task<IReadOnlyList<OrderShop>> GetOrderShopByUserEmailAsync(string email);
        Task<OrderShop> GetOrderShopOrderByIdAsync(int id, string email);
        Task<IReadOnlyList<ShippingType>> GetShippingTypeAsync();
    }
}
