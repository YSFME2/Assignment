using Domain.Abstractions.Repositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepositories
    {
        public ProductRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<Product>> GetFilterAsync(string? name = null, string? category = null)
        {
            var query = _dbSet.AsQueryable();
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(x => x.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase));
            }

            if (string.IsNullOrWhiteSpace(category))
                return await query.ToListAsync();

            if (int.TryParse(category, out var categoryId))
            {
                query = query.Where(x => x.CategoryId == categoryId || x.Category.Name.Contains(category));
            }
            else
            {
                query = query.Where(x => x.Category.Name.Contains(category));
            }

            return await query.ToListAsync();
        }
    }
}
