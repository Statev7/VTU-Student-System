namespace StudentSystem.Services.Data.Features.Payments.Services.Implementation
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.Extensions.Logging;

    using Stripe.Checkout;

    using StudentSystem.Common.Infrastructure.Cache.Services.Contracts;
    using StudentSystem.Common.Infrastructure.Cache.Settings;
    using StudentSystem.Common.Infrastructure.Extensions;
    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Data.Models.Courses.Enums;
    using StudentSystem.Services.Data.Features.Courses.DTOs.ServiceModels;
    using StudentSystem.Services.Data.Features.Courses.Services.Contracts;
    using StudentSystem.Services.Data.Features.Payments.DTOs.ServiceModels;
    using StudentSystem.Services.Data.Features.Payments.Services.Contracts;
    using StudentSystem.Services.Data.Features.StudentCourses.Services.Contracts;
    using StudentSystem.Services.Data.Features.Students.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Services;
    using StudentSystem.Services.Data.Infrastructure.Services.Contracts;

    using static StudentSystem.Common.Constants.NotificationConstants;

    public class PaymentService : BaseService<Payment>, IPaymentService
    {
        private readonly ICourseService courseService;
        private readonly ICurrentUserService currentUserService;
        private readonly IStudentService studentService;
        private readonly IStudentCourseService studentCourseService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<PaymentService> logger;
        private readonly SessionService sessionService;

        public PaymentService(
            IRepository<Payment> repository,
            IMapper mapper,
            ICourseService courseService,
            ICurrentUserService currentUserService,
            IStudentService studentService,
            IStudentCourseService studentCourseService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<PaymentService> logger)
            : base(repository, mapper)
        {
            this.courseService = courseService;
            this.currentUserService = currentUserService;
            this.studentService = studentService;
            this.studentCourseService = studentCourseService;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;

            this.sessionService = new SessionService();
        }

        public async Task<Result<SessionServiceModel>> CheckOutAsync(Guid courseId)
        {
            var isCourseNotExist = !await this.courseService.IsExistAsync(x => x.Id.Equals(courseId));
            if (isCourseNotExist)
            {
                return Result<SessionServiceModel>.Failure(InvalidCourseErrorMessage);
            }

            var isCourseNotActiveOrAlreadyStarted = await this.courseService.IsExistAsync(
                x => x.Id.Equals(courseId) && 
                (!x.IsActive || DateTime.UtcNow > x.StartDate));

            if (isCourseNotActiveOrAlreadyStarted)
            {
                return Result<SessionServiceModel>.Failure(NotActiveCourseErrorMessage);
            }

            var isStudentAlreadyBuyTheCourse = await this.studentCourseService.IsUserRegisteredInCourseAsync(courseId, this.currentUserService.GetUserId());
            if (isStudentAlreadyBuyTheCourse)
            {
                return Result<SessionServiceModel>.Failure(StudentAlreadyInCourseMessage);
            }

            var sessionOptions = await this.CreateSessionOptionsAsync(courseId);

            var session = await this.sessionService.CreateAsync(sessionOptions);

            return Result<SessionServiceModel>.SuccessWith(new SessionServiceModel() { Url = session.Url });
        }

        public async Task<Result> ProcessThePaymentAsync(string sessionId, Guid courseId)
        {
            var session = await this.sessionService.GetAsync(sessionId);

            var isPaymentStatusValueValid = Enum.TryParse<PaymentStatus>(session.PaymentStatus, true, out var paymentStatus);

            if (!isPaymentStatusValueValid)
            {
                this.logger.LogError($"{session.PaymentStatus} is not a valid payment status.");

                return ErrorMesage;
            }

            var studentId = await this.studentService.GetIdByUserIdAsync(this.currentUserService.GetUserId());

            var payment = new Payment()
            {
                CourseId = courseId,
                StudentId = studentId,
                SessionId = sessionId,
                Status = paymentStatus,
            };

            await this.Repository.AddAsync(payment);
            await this.Repository.SaveChangesAsync();

            var paymentValue = paymentStatus.GetEnumValue();
            var isPaymentSuccess = paymentValue.Equals(PaymentStatus.Paid.GetEnumValue());

            if (isPaymentSuccess)
            {
                await this.studentService.SetActiveStatusAsync(studentId, true);
            }

            var result = isPaymentSuccess
                ? Result.Success(SuccessfullyPaymentMessage)
                : UnsuccessfullyPaymentMessage;

            return result; 
        }

        private async Task<SessionCreateOptions> CreateSessionOptionsAsync(Guid courseId)
        {
            var course = await this.courseService.GetByIdAsync<CoursePaymentDetailsServiceModel>(courseId);

            var orderConfirmationUrl = this.GenerateOrderConfirmationUrl(courseId);
            var hostUrl = this.GenerateHostUrl();

            if (string.IsNullOrEmpty(orderConfirmationUrl) || string.IsNullOrEmpty(hostUrl))
            {
                return new SessionCreateOptions();
            }

            var options = new SessionCreateOptions
            {
                SuccessUrl = orderConfirmationUrl,
                CancelUrl = $"{hostUrl}/Home",
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new()
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmountDecimal = course.Price * 100,
                            Currency = "BGN",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = course.Name,
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
            };

            return options;
        }

        private string GenerateOrderConfirmationUrl(Guid courseId)
        {
            var fullPath = this.httpContextAccessor?.HttpContext?.Request.GetEncodedUrl();

            if (string.IsNullOrEmpty(fullPath))
            {
                return string.Empty;
            }

            var lastSlashIndex = fullPath.LastIndexOf('/');

            fullPath = fullPath.Substring(0, lastSlashIndex);

            fullPath += "/OrderConfirmation?sessionId=" + "{CHECKOUT_SESSION_ID}" + $"&courseId={courseId}";

            return fullPath;
        }

        private string GenerateHostUrl()
            => $"{this.httpContextAccessor?.HttpContext?.Request.Scheme}://{this.httpContextAccessor?.HttpContext?.Request.Host}";
    }
}
