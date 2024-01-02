namespace Application.Abstractions
{
    public interface IAppDbContext
    {
        DbSet<Product> Products { get; }
        DbSet<Category> Categories { get; }

        int SaveChangesAsync();
    }
}
