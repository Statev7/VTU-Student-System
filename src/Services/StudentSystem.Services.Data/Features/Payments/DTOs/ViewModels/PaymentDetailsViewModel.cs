namespace StudentSystem.Services.Data.Features.Payments.DTOs.ViewModels
{
    using StudentSystem.Data.Models.Courses.Enums;

    public class PaymentDetailsViewModel
    {
        public string CourseName { get; set; } = null!;

        public PaymentStatus Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public string SessionId { get; set; }

        public PaymentShippingViewModel PaymentShipping { get; set; }

        public string PurchaseDateFormated => this.CreatedOn.ToString("HH:mm dd.MM.yyyy");
    }
}
