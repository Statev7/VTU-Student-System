namespace StudentSystem.Data.Seed.Seeds
{
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;

    using StudentSystem.Data.Models.Users;

    public class TeacherSeeder : BaseSeeder<Teacher>
    {
        public TeacherSeeder(IServiceScope serviceScope, string jsonData) 
            : base(serviceScope, jsonData)
        {
        }

        public async override Task SeedAsync()
        {
            if (await this.IsAlreadySeedAsync())
            {
                return;
            }

            var teachers = JsonConvert.DeserializeObject<IEnumerable<Teacher>>(this.JsonData);

            await this.DbSet.AddRangeAsync(teachers);
            await this.DbContext.SaveChangesAsync();
        }
    }
}
