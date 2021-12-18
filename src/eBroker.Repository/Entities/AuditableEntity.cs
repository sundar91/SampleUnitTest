namespace eBroker.Data.Entities
{
    public class AuditableEntity
    {
        public string CreatedBy { get; set; } =  "system";
        public DateTime Created { get; set;} = DateTime.Now;
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public bool IsDeleted { get; set; }

    }
}
