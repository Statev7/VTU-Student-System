namespace StudentSystem.Services.Data.Common.Services.Implementation
{
    using Microsoft.AspNetCore.Http;

    using StudentSystem.Common.Infrastructure;
    using StudentSystem.Services.Data.Common.Services.Contracts;

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
            => this.httpContextAccessor.HttpContext.User.GetUserId();
    }
}
