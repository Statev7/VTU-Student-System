namespace StudentSystem.Data.Models.Courses
{
    using System.ComponentModel.DataAnnotations;

    using StudentSystem.Data.Common.Models;

    using static StudentSystem.Data.Common.Constants.Exam;

    public class Exam : BaseModel
    {
        public int? Grade { get; set; }

        [StringLength(CommentMaxLength)]
        public string Comment { get; set; }

        public Guid CourseId { get; set; }

        public Guid StudentId { get; set; }

        public CourseStudentMap CourseStudentMap { get; set; }
    }
}
