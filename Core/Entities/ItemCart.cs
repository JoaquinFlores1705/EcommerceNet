using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ItemCart
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public string Image { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
    }
}
