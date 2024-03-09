namespace StudentSystem.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static StudentSystem.Common.Constants.GlobalConstants;
    using static StudentSystem.Web.Infrastructure.Constants;

    [Area(AdministrationArea)]
    [Authorize(Roles = AdminRole)]
    public class BaseAdminController : Controller
    {

    }
}
