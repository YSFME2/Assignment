namespace Application.Abstractions
{
    public interface IAppDbContext
    {
        DbSet<Product> Products { get; }
        DbSet<Category> Categories { get; }
        DbSet<CartItem> CartItems { get; }
    }
}
