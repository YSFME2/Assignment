namespace Domain.Entities
{
    public class CartItem : AuditableEntity
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public string UserId { get; set; } = null!;
        public virtual AppUser User { get; set; }
    }
}
