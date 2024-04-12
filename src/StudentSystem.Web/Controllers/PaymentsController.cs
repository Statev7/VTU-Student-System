namespace StudentSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using StudentSystem.Services.Data.Features.Payments.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure.Services.Contracts;
    using StudentSystem.Web.Infrastructure.Extensions;
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;

    using static StudentSystem.Common.Constants.GlobalConstants;

    [Authorize(Roles = StudentRole)]
    public class PaymentsController : Controller
    {
        private readonly IPaymentService paymentService;
        private readonly IStudentCourseService studentCourseService;
        private readonly IControllerHelper controllerHelper;

        public PaymentsController(
            IPaymentService paymentService,
            IStudentCourseService studentCourseService,
            IControllerHelper controllerHelper)
        {
            this.paymentService = paymentService;
            this.studentCourseService = studentCourseService;
            this.controllerHelper = controllerHelper;
        }

        [HttpGet]
        public async Task<IActionResult> CheckOut(Guid courseId)
        {
            var result = await paymentService.CheckOutAsync(courseId);

            if (!result.Succeed)
            {
                this.TempData.Add(result);

                return this.RedirectToAction(nameof(HomeController.Index), this.controllerHelper.GetName(nameof(HomeController)), new { area = "" });
            }

            return this.Redirect(result.Data.Url);
        }

        [HttpGet]
        public async Task<IActionResult> OrderConfirmation(string sessionId, Guid courseId)
        {
            var result = await this.paymentService.ProcessThePaymentAsync(sessionId, courseId);

            if (result.Succeed)
            {
                await this.studentCourseService.AddStudentToCourseAsync(courseId);
            }

            this.TempData.Add(result);

            return this.RedirectToAction(nameof(HomeController.Index), this.controllerHelper.GetName(nameof(HomeController)), new { area = "" });
        }
    }
}
