namespace StudentSystem.Web.Areas.Students.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Data.Features.Payments.Services.Contracts;
    using StudentSystem.Services.Data.Features.Students.Services.Contracts;

    using static StudentSystem.Common.Constants.GlobalConstants;

    [Authorize(Roles = StudentRole)]
    public class ProfileController : BaseStudentController
    {
        private readonly IStudentService studentService;
        private readonly IPaymentService paymentService;

        public ProfileController(IStudentService studentService, IPaymentService paymentService)
        {
            this.studentService = studentService;
            this.paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> Trainings()
        {
            var model = await this.studentService.GetTrainingsInformationAsync();

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Payments(int currentPage = 1)
        {
            var payments = await this.paymentService.GetStudentPaymentsAsync(currentPage);

            return this.View(payments);
        }
    }
}
