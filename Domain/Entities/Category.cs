using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Category : AuditableEntity
    {
        [Length(3,150)]
        public string Name { get; set; } = null!;
        [MaxLength(500)]
        public string? Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        [NotMapped]
        public Product Pro { get; set; }
    }
}
