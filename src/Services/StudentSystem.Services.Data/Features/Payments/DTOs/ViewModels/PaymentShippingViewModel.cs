namespace StudentSystem.Services.Data.Features.Payments.DTOs.ViewModels
{
    public class PaymentShippingViewModel
    {
        public string PaymentMethod { get; set; } = null!;

        public string CustomerEmail { get; set; } = null!;

        public long AmountTotal { get; set; }

        public string Currency { get; set; }
    }
}
