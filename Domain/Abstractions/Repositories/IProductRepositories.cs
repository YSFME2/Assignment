namespace Domain.Abstractions.Repositories
{
    public interface IProductRepositories : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetFilterAsync(string? name, string? category);
    }
}
