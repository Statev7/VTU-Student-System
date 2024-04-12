namespace StudentSystem.Data.Models.Courses.Enums
{
    using StudentSystem.Common.Infrastructure.Attributes;

    public enum PaymentStatus
    {
        [CustomizeEnum("", "paid")]
        Paid = 2,
        [CustomizeEnum("", "unpaid")]
        Unpaid = 4,
        [CustomizeEnum("", "no_payment_required")]
        NoPaymentRequired = 8,
    }
}
