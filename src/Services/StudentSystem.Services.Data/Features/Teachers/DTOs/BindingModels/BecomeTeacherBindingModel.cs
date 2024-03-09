namespace StudentSystem.Services.Data.Features.Teachers.DTOs.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    using static StudentSystem.Data.Common.Constants.ApplicationUser;
    using static StudentSystem.Data.Common.Constants.Teacher;

    public class BecomeTeacherBindingModel
    {
        [Required]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(AboutMeMaxLength)]
        public string AboutMe { get; set; } = null!;
    }
}
