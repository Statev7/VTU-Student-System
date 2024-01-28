namespace StudentSystem.Data.Common.Models
{
    public abstract class BaseModel : IAuditInfo
    {
        protected BaseModel()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
