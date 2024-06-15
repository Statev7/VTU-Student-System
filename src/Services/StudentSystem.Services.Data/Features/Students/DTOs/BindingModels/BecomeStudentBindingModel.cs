namespace StudentSystem.Services.Data.Features.Students.DTOs.BindingModels
{
    using System.ComponentModel.DataAnnotations;
    using StudentSystem.Services.Data.Features.City.DTOs.ViewModels;

    using static StudentSystem.Data.Common.Constants.Student;
    using static StudentSystem.Data.Common.Constants.ApplicationUser;

    public class BecomeStudentBindingModel
    {
        [Required]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(PhoneNumberMaxLength)]
        [RegularExpression(PhoneNumberRegex, ErrorMessage = "The number is not a valid one!")]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public Guid CityId { get; set; }

        public IEnumerable<CityViewModel> Cities { get; set; } = new List<CityViewModel>();
    }
}
