﻿namespace StudentSystem.Services.Data.Features.Payments.Services.Contracts
{
    using StudentSystem.Services.Data.Features.Payments.DTOs.ServiceModels;
    using StudentSystem.Services.Data.Infrastructure;

    public interface IPaymentService
    {
        Task<Result<SessionServiceModel>> CheckOutAsync(Guid courseId);

        Task<Result> ProcessThePaymentAsync(string sessionId, Guid courseId);
    }
}
