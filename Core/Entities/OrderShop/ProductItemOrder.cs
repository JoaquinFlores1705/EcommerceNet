using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrderShop
{
    public class ProductItemOrder
    {
        public ProductItemOrder()
        {
            
        }
        public ProductItemOrder(int productItemId, string productName, string imageUrl)
        {
            this.ProductItemId = productItemId;
            this.ProductName = productName;
            this.ImageUrl = imageUrl;
        }
        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
    }
}
