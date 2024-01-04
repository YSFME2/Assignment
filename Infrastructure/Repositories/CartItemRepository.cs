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
            _queryable = _queryable.Include(x => x.Product);
        }

        public async Task<CartItem> GetUserItemByIdAsync(int cartItemId, string userId)
        {
            return await _queryable.FirstOrDefaultAsync(x=>x.Id == cartItemId && x.UserId == userId);
        }

        public async Task<IEnumerable<CartItem>> GetUserItemsAsync(string userId)
        {
            return await _queryable.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}
