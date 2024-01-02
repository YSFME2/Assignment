namespace Domain.Entities
{
    public class Category : AuditableEntity
    {
        [MaxLength(200)]
        public string Name { get; set; } = null!;
        [MaxLength(500)]
        public string? Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
