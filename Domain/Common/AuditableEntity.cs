namespace Domain.Common
{
    public abstract class AuditableEntity
    {
        public int Id { get; set; }


        public DateTimeOffset CreatedOn { get; set; }
        public string? CreatedById { get; set; }
        public virtual AppUser? CreatedBy { get; set; }


        public DateTimeOffset? LastModifiedOn { get; set; }
        public string? LastModifiedById { get; set; }
        public virtual AppUser? LastModifiedBy { get; set; }


        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public string? DeletedById { get; set; }
        public virtual AppUser? DeletedBy { get; set; }

    }
}
