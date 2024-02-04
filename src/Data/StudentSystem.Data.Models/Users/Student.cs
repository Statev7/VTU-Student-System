namespace StudentSystem.Data.Models.Users
{
    using System.ComponentModel.DataAnnotations;

    using StudentSystem.Data.Common.Models;

    using static StudentSystem.Data.Common.Constants.Student;

    public class Student : BaseModel
    {
        [StringLength(FirstNameMaxLength)]
        public string? FirstName { get; set; }

        [StringLength(LastNameMaxLength)]
        public string? LastName { get; set; }

        public Guid? CityId { get; set; }

        public City City { get; set; }

        public bool IsApproved { get; set; }

        public bool IsApplied { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = null!;

        public ApplicationUser User { get; set; }
    }
}
