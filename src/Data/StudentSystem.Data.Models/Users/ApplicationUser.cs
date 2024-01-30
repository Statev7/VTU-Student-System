namespace StudentSystem.Data.Models.Users
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Identity;

    using StudentSystem.Data.Common.Models;

    using static StudentSystem.Data.Common.Constants.User;

    public class ApplicationUser : IdentityUser, IAuditInfo
    {
        [StringLength(FirstNameMaxLength)]
        public string? FirstName { get; set; }

        [StringLength(LastNameMaxLength)]
        public string? LastName { get; set; }

        public Student? Student { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
