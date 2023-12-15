using Core.Entities;
using Core.Entities.OrderShop;
using Core.Interfaces;
using Core.Specifications;
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
        private readonly IShoppingCartRepository _shopCartRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderShopService(IShoppingCartRepository shopCartRepository, IUnitOfWork unitOfWork)
        {
            _shopCartRepository = shopCartRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderShop> AddOrderShopAsync(string buyerEmail, int shippingType, string cartId, Direction direction)
        {
            var cartShop = await _shopCartRepository.GetShoppingCartAsync(cartId);

            var items = new List<OrderItem>();

            foreach (var item in cartShop.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var orderedItem = new ProductItemOrder(productItem.Id, productItem.Name, productItem.Image);
                var orderItem = new OrderItem(orderedItem, productItem.Price, item.Amount);
                items.Add(orderItem);
            }

            var shippingTypeEntity = await _unitOfWork.Repository<ShippingType>().GetByIdAsync(shippingType);

            var subtotal = items.Sum(i => i.Price * i.Amount);

            var orderShop = new OrderShop(buyerEmail, direction, shippingTypeEntity, items,subtotal);

            _unitOfWork.Repository<OrderShop>().AddEntity(orderShop);

            var result = await _unitOfWork.Complete();

            if(result <= 0)
            {
                return null;
            }

            await _shopCartRepository.DeleteShoppingCartAsync(cartId);

            return orderShop;
        }

        public async Task<IReadOnlyList<OrderShop>> GetOrderShopByUserEmailAsync(string email)
        {
            var spec = new OrderShopWithItemsSpecification(email);
            return await _unitOfWork.Repository<OrderShop>().GetAllWithSpec(spec);
        }

        public async Task<OrderShop> GetOrderShopOrderByIdAsync(int id, string email)
        {
            var spec = new OrderShopWithItemsSpecification(id, email);
            return await _unitOfWork.Repository<OrderShop>().GetByIdWithSpec(spec);
        }

        public async Task<IReadOnlyList<ShippingType>> GetShippingTypeAsync()
        {
            return await _unitOfWork.Repository<ShippingType>().GetAllAsync();
        }
    }
}
