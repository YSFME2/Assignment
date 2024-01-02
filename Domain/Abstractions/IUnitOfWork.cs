using Domain.Abstractions.Repositories;

namespace Domain.Abstractions
{
    public interface IUnitOfWork
    {
        IGenericRepository<Category> GenericRepository { get; }
        IProductRepositories ProductRepository { get; }
        ICartItemRepository ICartItemRepository { get; }

        Task<int> Complete();
    }
}
