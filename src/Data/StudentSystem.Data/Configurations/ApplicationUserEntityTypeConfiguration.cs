namespace StudentSystem.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using StudentSystem.Data.Models.Users;

    public class ApplicationUserEntityTypeConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder
                .HasOne(au => au.Student)
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.ApplicationUserId);
        }
    }
}
