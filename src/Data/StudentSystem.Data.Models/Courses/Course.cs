namespace StudentSystem.Data.Models.Courses
{
    using System.ComponentModel.DataAnnotations;

    using StudentSystem.Data.Common.Models;
    using StudentSystem.Data.Models.Courses.Enums;
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
        [StringLength(ImageFolderMaxLength)]
        public string ImageFolder { get; set; } = null!;

        public decimal Price { get; set; }

        [Required]
        public Guid TeacherId { get; set; }

        public Teacher Teacher { get; set; }

        public ICollection<CourseStudentMap> Students { get; set; } = new HashSet<CourseStudentMap>();

        public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();

        public ICollection<Lesson> Lessons { get; set; } = new HashSet<Lesson>();
    }
}
