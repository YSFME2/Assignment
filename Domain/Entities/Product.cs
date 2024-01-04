
namespace Domain.Entities
{
    public class Product : AuditableEntity
    {
        [MaxLength(250)]
        public string Name { get; set; } = null!;
        [MaxLength(1000)]
        public string? Description { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        [Range(0, 1)]
        public decimal DiscountPercent { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
