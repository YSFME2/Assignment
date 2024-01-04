namespace Domain.Abstractions.Repositories
{
    public interface IProductRepositories : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetFilterAsync(string? filterText = null,
            int? categoryId = null,
            decimal? priceFrom = null,
            decimal? priceTo = null);
    }
}
