using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductWithCategoryAndBrandSpecification : BaseSpecification<Product>
    {
        public ProductWithCategoryAndBrandSpecification()
        {
            AddInclude( p => p.Brand );
            AddInclude( p => p.Category );
        }

        public ProductWithCategoryAndBrandSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(p => p.Brand );
            AddInclude(p => p.Category );
        }
    }
}
