using Core.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BussinessLogic.Data
{
    public class MarketDbContextData
    {
        public static async Task LoadDataAsync(MarketDbContext context, ILoggerFactory loggerFactory)
        {
			try
			{
				if (!context.Brand.Any())
				{
					var brandData = File.ReadAllText("../BussinessLogic/LoadData/brand.json");
					var brands = JsonSerializer.Deserialize<List<Brand>>(brandData);

					foreach (var brand in brands)
					{
						context.Brand.Add(brand);
					}

					context.SaveChangesAsync();
				}

                if (!context.Category.Any())
                {
                    var categoryData = File.ReadAllText("../BussinessLogic/LoadData/category.json");
                    var categorys = JsonSerializer.Deserialize<List<Category>>(categoryData);

                    foreach (var category in categorys)
                    {
                        context.Category.Add(category);
                    }

                    context.SaveChangesAsync();
                }

                if (!context.Product.Any())
                {
                    var productData = File.ReadAllText("../BussinessLogic/LoadData/product.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productData);

                    foreach (var product in products)
                    {
                        context.Product.Add(product);
                    }

                    context.SaveChangesAsync();
                }
            }
			catch (Exception e)
			{
                var logger = loggerFactory.CreateLogger<MarketDbContextData>();
                logger.LogError(e.Message);
			}
        }
    }
}
