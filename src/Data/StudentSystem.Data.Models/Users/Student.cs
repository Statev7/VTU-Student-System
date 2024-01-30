namespace StudentSystem.Data.Models.Users
{
    using System.ComponentModel.DataAnnotations;

    using StudentSystem.Data.Common.Models;

    public class Student : BaseModel
    {
        [Required]
        public string ApplicationUserId { get; set; } = null!;

        public ApplicationUser User { get; set; }
    }
}
