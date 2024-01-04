namespace Domain.Abstractions.Repositories
{
    public interface ICartItemRepository : IGenericRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetUserItemsAsync(string userId);
        Task<CartItem> GetUserItemByIdAsync(int cartItemId,string userId);
    }
}
