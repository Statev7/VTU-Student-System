namespace StudentSystem.Data.Models.Courses
{
    using System.ComponentModel.DataAnnotations;

    using StudentSystem.Data.Common.Models;

    using static StudentSystem.Data.Common.Constants.Resources;

    public class Resource : BaseModel
    {
        [Required]
        [StringLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(FolderPathMaxLength)]
        public string FolderPath { get; set; } = null!;

        [Required]
        [StringLength(ExtansionMaxLength)]
        public string Extension { get; set; } = null!;

        public Lesson Lesson { get; set; }

        [Required]
        public Guid LessonId { get; set; }
    }
}
