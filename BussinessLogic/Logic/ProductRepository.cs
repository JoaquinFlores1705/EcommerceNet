using BussinessLogic.Data;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Logic
{
    public class ProductRepository : IProductRepository
    {
        private readonly MarketDbContext _context;
        public ProductRepository(MarketDbContext context)
        {
            _context = context;
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Product
                            .Include(p => p.Brand)
                            .Include(p => p.Category)
                            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await _context.Product
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .ToListAsync();
        }
    }
}
