namespace StudentSystem.Services.Data.Infrastructure.Services.Implementation
{
    using Microsoft.AspNetCore.Http;
    using StudentSystem.Common.Infrastructure.Extensions;
    using StudentSystem.Services.Data.Infrastructure.Services.Contracts;

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor) 
            => this.httpContextAccessor = httpContextAccessor;

        public string GetUserId() => this.httpContextAccessor.HttpContext.User.GetUserId();
    }
}
