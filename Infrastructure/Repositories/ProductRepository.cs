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
        public ProductRepository(AppDbContext dbContext) : base(dbContext)
        {
            _queryable = _queryable.Include(x => x.Category);
        }

        public async Task<IEnumerable<Product>> GetFilterAsync(
            string? filterText = null,
            int? categoryId = null,
            decimal? priceFrom = null,
            decimal? priceTo = null)
        {
            var query = _queryable.AsQueryable();
            
            if (categoryId > 0)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }

            if (priceFrom >= 0)
            {
                query = query.Where(x => x.Price * (1 - x.DiscountPercent) >= priceFrom);
            }

            if (priceTo >= 0)
            {
                query = query.Where(x => x.Price * (1 - x.DiscountPercent) <= priceTo);
            }

            if (!string.IsNullOrWhiteSpace(filterText))
            {
                filterText = filterText.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(filterText)
                || (x.Description != null && x.Description.ToLower().Contains(filterText))
                || x.Category.Name.ToLower().Contains(filterText));

                return await query.OrderByDescending(x => x.Name.Contains(filterText)).ToListAsync();
            }

            return await query.ToListAsync();
        }
    }
}
