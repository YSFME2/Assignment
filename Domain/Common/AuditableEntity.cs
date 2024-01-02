namespace Domain.Common
{
    public class AuditableEntity
    {
        public int Id { get; set; }


        public DateTime CreatedOn { get; set; }
        public string? CreatedById { get; set; }
        public AppUser? CreatedBy { get; set; }


        public DateTime? LastModifiedOn { get; set; }
        public string? LastModifiedById { get; set; }
        public AppUser? LastModifiedBy { get;set; }


        public bool IsDeleted { get; set; }
        public string? DeletedById { get; set; }
        public AppUser? DeletedBy { get; set; }

    }
}
