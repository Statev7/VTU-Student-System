namespace StudentSystem.Services.Data.Features.Students.DTOs.ViewModels
{
    public class PendingStudentViewModel
    {
        public string FullName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string CityName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public DateTime AppliedOn { get; set; }
    }
}
