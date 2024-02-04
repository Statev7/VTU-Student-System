namespace StudentSystem.Data.Models.Users
{
    using System;

    using Microsoft.AspNetCore.Identity;

    using StudentSystem.Data.Common.Models;


    public class ApplicationUser : IdentityUser, IAuditInfo
    {
        public Student? Student { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
