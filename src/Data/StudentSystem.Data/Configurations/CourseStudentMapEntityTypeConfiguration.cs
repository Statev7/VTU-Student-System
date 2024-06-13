namespace StudentSystem.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using StudentSystem.Data.Models.Courses;

    public class CourseStudentMapEntityTypeConfiguration : IEntityTypeConfiguration<CourseStudentMap>
    {
        public void Configure(EntityTypeBuilder<CourseStudentMap> builder)
        {
            builder.HasKey(cs => new { cs.CourseId, cs.StudentId } );

            builder
                .HasOne(cs => cs.Student)
                .WithMany(s => s.Courses);

            builder
                .HasOne(cs => cs.Course)
                .WithMany(c => c.Students);

            builder
                .HasOne(csm => csm.Exam)
                .WithOne(e => e.CourseStudentMap)
                .HasForeignKey<Exam>(e => new { e.CourseId, e.StudentId })
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
