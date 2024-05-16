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
                .HasForeignKey<Student>(s => s.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(au => au.Teacher)
                .WithOne(t => t.User)
                .HasForeignKey<Teacher>(s => s.ApplicationUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                 .HasMany(e => e.UserRoles)
                 .WithOne(e => e.User)
                 .HasForeignKey(ur => ur.UserId)
                 .IsRequired();

            builder
                .HasIndex(u => u.Email)
                .IsUnique(true);
        }
    }
}
