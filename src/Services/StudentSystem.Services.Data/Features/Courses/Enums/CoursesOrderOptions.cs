namespace StudentSystem.Services.Data.Features.Courses.Enums
{
    using StudentSystem.Common.Infrastructure.Attributes;

    public enum CoursesOrderOptions
    {   
        [CustomizeEnum("Ascending - Alphabetically", "Asc - Name")]
        AscName,

        [CustomizeEnum("Descending - Alphabeticall", "Desc - Name")]
        DescName,

        [CustomizeEnum("Ascending - Start Date", "Asc - StartDate")]
        AscStartDate,

        [CustomizeEnum("Descending - Start Date", "Desc - StartDate")]
        DescStartDate,
    }
}
