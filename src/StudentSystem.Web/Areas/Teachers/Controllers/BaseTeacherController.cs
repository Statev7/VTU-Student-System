namespace StudentSystem.Web.Areas.Teachers.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using static StudentSystem.Web.Infrastructure.Constants;
    using static StudentSystem.Common.Constants.GlobalConstants;

    [Area(TeachersArea)]
    [Authorize(Roles = TeacherRole)]
    public abstract class BaseTeacherController : Controller
    {
        
    }
}
