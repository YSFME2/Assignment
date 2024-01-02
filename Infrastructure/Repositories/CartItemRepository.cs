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
    public class CartItemRepository :GenericRepository<CartItem>, ICartItemRepository
    {

        public CartItemRepository(AppDbContext dbContext):base(dbContext) 
        {
        }

        public async Task<IEnumerable<CartItem>> GetUserItemsAsync(string userId)
        {
            return await _dbSet.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
