namespace StudentSystem.Data.Models.Users
{
    using System.ComponentModel.DataAnnotations;

    using StudentSystem.Data.Common.Models;
    using StudentSystem.Data.Models.Courses;

    using static StudentSystem.Data.Common.Constants.Teacher;

    public class Teacher : BaseModel
    {
        [Required]
        [StringLength(AboutMeMaxLength)]
        public string AboutМe { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = null!;

        public ApplicationUser User { get; set; }

        public ICollection<Course> Courses { get; set; } = new HashSet<Course>();
    }
}
