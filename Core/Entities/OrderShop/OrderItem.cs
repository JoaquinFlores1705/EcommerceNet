using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrderShop
{
    public class OrderItem : BaseClass
    {
        public OrderItem()
        {
            
        }
        public OrderItem(ProductItemOrder itemOrder, decimal price, int amount)
        {
            this.ItemOrder = itemOrder;
            this.Price = price;
            this.Amount = amount;
        }
        public ProductItemOrder ItemOrder { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
    }
}
