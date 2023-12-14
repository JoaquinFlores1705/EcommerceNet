using Core.Entities;
using Core.Entities.OrderShop;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Direction = Core.Entities.OrderShop.Direction;

namespace BussinessLogic.Logic
{
    public class OrderShopService : IOrderShopService
    {
        private readonly IGenericRepository<OrderShop> _orderShopRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IShoppingCartRepository _shopCartRepository;
        private readonly IGenericRepository<ShippingType> _shippingTypeRepository;

        public OrderShopService(IGenericRepository<OrderShop> orderShopRepository, IGenericRepository<Product> productRepository, IShoppingCartRepository shopCartRepository, IGenericRepository<ShippingType> shippingTypeRepository)
        {
            _orderShopRepository = orderShopRepository;
            _productRepository = productRepository;
            _shopCartRepository = shopCartRepository;
            _shippingTypeRepository = shippingTypeRepository;
        }

        public async Task<OrderShop> AddOrderShopAsync(string buyerEmail, int shippingType, string cartId, Direction direction)
        {
            var cartShop = await _shopCartRepository.GetShoppingCartAsync(cartId);

            var items = new List<OrderItem>();

            foreach (var item in cartShop.Items)
            {
                var productItem = await _productRepository.GetByIdAsync(item.Id);
                var orderedItem = new ProductItemOrder(productItem.Id, productItem.Name, productItem.Image);
                var orderItem = new OrderItem(orderedItem, productItem.Price, item.Amount);
                items.Add(orderItem);
            }

            var shippingTypeEntity = await _shippingTypeRepository.GetByIdAsync(shippingType);

            var subtotal = items.Sum(i => i.Price * i.Amount);

            var orderShop = new OrderShop(buyerEmail, direction, shippingTypeEntity, items,subtotal);

            return orderShop;
        }

        public Task<IReadOnlyList<OrderShop>> GetOrderShopByUserEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<OrderShop> GetOrderShopOrderByIdAsync(int id, string email)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ShippingType>> GetShippingTypeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
