using Domain.Abstractions.Repositories;

namespace Domain.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Category> CategoryRepository { get; }
        IProductRepositories ProductRepository { get; }
        ICartItemRepository CartItemRepository { get; }

        Task<int> Complete();
    }
}
