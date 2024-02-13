namespace StudentSystem.Services.Data.Infrastructure.Services.Implementation
{
    using Microsoft.AspNetCore.Http;

    using StudentSystem.Common.Infrastructure;
    using StudentSystem.Services.Data.Infrastructure.Services.Contracts;

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
            => httpContextAccessor.HttpContext.User.GetUserId();
    }
}
