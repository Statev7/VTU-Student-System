namespace StudentSystem.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using StudentSystem.Data.Models.Courses;

    public class CourseEntityTypeConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder
                .HasOne(c => c.ImageFile)
                .WithOne(i => i.Course)
                .HasForeignKey<Course>(c => c.ImageFileId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
