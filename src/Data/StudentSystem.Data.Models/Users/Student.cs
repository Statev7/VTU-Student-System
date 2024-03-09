﻿namespace StudentSystem.Data.Models.Users
{
    using System.ComponentModel.DataAnnotations;

    using StudentSystem.Data.Common.Models;

    public class Student : BaseModel
    {
        public Guid? CityId { get; set; }

        public City City { get; set; }

        public bool IsApproved { get; set; }

        public bool IsApplied { get; set; }

        [Required]
        public string ApplicationUserId { get; set; } = null!;

        public ApplicationUser User { get; set; }
    }
}
