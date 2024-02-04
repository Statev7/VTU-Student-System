namespace StudentSystem.Data.Models.Users
{
    using System.ComponentModel.DataAnnotations;

    using StudentSystem.Data.Common.Models;

    using static StudentSystem.Data.Common.Constants.City;

    public class City : BaseModel
    {
        public City()
        {
            this.Students = new HashSet<Student>();
        }

        [Required]
        [StringLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public ICollection<Student> Students { get; set; }
    }
}
