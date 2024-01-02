using Domain.Abstractions.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Repositories;

namespace Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        IGenericRepository<Category> categoryRepository;
        IProductRepositories productRepository;
        ICartItemRepository cartItemRepository;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IGenericRepository<Category> CategoryRepository => categoryRepository ??= new GenericRepository<Category>(_dbContext);

        public IProductRepositories ProductRepository => productRepository ??= new ProductRepository(_dbContext);

        public ICartItemRepository CartItemRepository => cartItemRepository ??= new CartItemRepository(_dbContext);

        public async Task<int> Complete()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
    }
}
