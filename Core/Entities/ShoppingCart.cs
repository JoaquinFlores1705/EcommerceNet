using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ShoppingCart
    {

        public ShoppingCart()
        {
            
        }

        public ShoppingCart(string id)
        {
            Id = id;
        }
        public string Id { get; set; }
        public List<ItemCart> Items { get; set; } = new List<ItemCart>();


    }
}
