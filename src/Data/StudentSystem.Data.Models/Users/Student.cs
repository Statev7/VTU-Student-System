﻿namespace StudentSystem.Data.Models.Users
{
    using System.ComponentModel.DataAnnotations;

    using StudentSystem.Data.Common.Models;
    using StudentSystem.Data.Models.Courses;

    using static StudentSystem.Data.Common.Constants.Student;

    public class Student : BaseModel
    {
        public Guid? CityId { get; set; }

        public City City { get; set; }

        [MaxLength(PhoneNumberMaxLength)]
        public string? PhoneNumber { get; set; }

        public bool IsApproved { get; set; }

        public bool IsApplied { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = null!;

        public ApplicationUser User { get; set; }

        public ICollection<CourseStudentMap> Courses { get; set; } = new List<CourseStudentMap>();

        public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
    }
}
