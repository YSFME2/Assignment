namespace Application.Models.Pagination
{
    public class PaginatedList<TEntity>
    {
        public IReadOnlyCollection<TEntity> Items { get; }
        public int PageNumber { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public PaginatedList(IReadOnlyCollection<TEntity> items, int count, int pageSize, int pageNumber)
        {
            Items = items;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
        }

        public static async Task<PaginatedList<TEntity>> CreateAsync(IQueryable<TEntity> source, int pageSize, int pageNumber = 1)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new(items, count, pageSize, pageNumber);
        }
    }
}
