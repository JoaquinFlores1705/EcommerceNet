using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductForCountingSpecification : BaseSpecification<Product>
    {
        public ProductForCountingSpecification(ProductSpecificationParams productsParams)
            : base(x =>
                        (string.IsNullOrEmpty(productsParams.Search) || x.Name.Contains(productsParams.Search)) &&
                        (!productsParams.Brand.HasValue || x.BrandId == productsParams.Brand) &&
                        (!productsParams.Category.HasValue || x.CategoryId == productsParams.Category)
            )
        { 
        
        }
    }
}
