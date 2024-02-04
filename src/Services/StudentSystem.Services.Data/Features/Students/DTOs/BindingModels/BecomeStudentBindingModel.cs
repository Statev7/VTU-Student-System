namespace StudentSystem.Services.Data.Features.Students.DTOs.BindingModels
{
    using System.ComponentModel.DataAnnotations;
    using StudentSystem.Services.Data.Features.City.DTOs.ViewModels;
    using static StudentSystem.Data.Common.Constants.Student;

    public class BecomeStudentBindingModel
    {
        [Required]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        public string LastName { get; set; } = null!;

        [Required]
        public Guid CityId { get; set; }

        public IEnumerable<CityViewModel> Cities { get; set; }
    }
}
