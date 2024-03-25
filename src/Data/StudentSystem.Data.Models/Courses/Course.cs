namespace StudentSystem.Data.Models.Courses
{
    using System.ComponentModel.DataAnnotations;

    using StudentSystem.Data.Common.Models;
    using StudentSystem.Data.Models.Users;

    using static StudentSystem.Data.Common.Constants.Course;

    public class Course : BaseModel
    {
        [Required]
        [StringLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(TeaserDescriptionMaxLength)]
        public string TeaserDescription { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        public ComplexityLevel Complexity { get; set; }

        public int Credits { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        public Guid TeacherId { get; set; }

        public Teacher Teacher { get; set; }

        public ICollection<CourseStudentMap> Students { get; set; } = new HashSet<CourseStudentMap>();
    }
}
