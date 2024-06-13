namespace StudentSystem.Data.Models.Courses
{
    using StudentSystem.Data.Common.Models;
    using StudentSystem.Data.Models.Users;

    public class CourseStudentMap : IAuditInfo
    {
        public Guid CourseId { get; set; }

        public Course Course { get; set; }

        public Guid StudentId { get; set; }

        public Student Student { get; set; }

        public Exam Exam { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
