namespace StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels
{
    public class CoursePaymentDetailsViewModel
    {
        public string Name { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public decimal Price { get; set; }
    }
}
