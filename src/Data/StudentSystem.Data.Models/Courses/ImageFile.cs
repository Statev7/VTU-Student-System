namespace StudentSystem.Data.Models.Courses
{
    using System.ComponentModel.DataAnnotations;

    using StudentSystem.Data.Common.Models;

    using static StudentSystem.Data.Common.Constants.ImageFile;

    public class ImageFile : BaseModel
    {
        [Required]
        [StringLength(FolderMaxLength)]
        public string Folder { get; set; } = null!;

        public Course Course { get; set; }
    }
}
