namespace StudentSystem.Data.Models.Courses
{
    using System.ComponentModel.DataAnnotations;

    using StudentSystem.Data.Common.Models;
    using StudentSystem.Data.Models.Courses.Enums;
    using StudentSystem.Data.Models.Users;

    using static StudentSystem.Data.Common.Constants.Payment;

    public class Payment : IAuditInfo
    {
        public Guid CourseId { get; set; }

        public Course Course { get; set; }

        public Guid StudentId { get; set; }

        public Student Student { get; set; }

        [Required]
        [StringLength(SessionMaxLength)]
        public string SessionId { get; set; }

        public PaymentStatus Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
