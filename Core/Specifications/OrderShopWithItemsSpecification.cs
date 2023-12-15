using Core.Entities.OrderShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class OrderShopWithItemsSpecification : BaseSpecification<OrderShop>
    {
        public OrderShopWithItemsSpecification(string email)
            :base( o => o.BuyerEmail==email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.ShippingType);
            AddOrderByDescending(o => o.OrderShopDate);
        }

        public OrderShopWithItemsSpecification(int id, string email)
            : base(o => o.BuyerEmail == email && 
                    o.Id == id        
            )
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.ShippingType);
            AddOrderByDescending(o => o.OrderShopDate);
        }
    }
}
