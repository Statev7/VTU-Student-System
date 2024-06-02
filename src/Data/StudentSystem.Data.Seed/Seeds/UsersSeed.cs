namespace StudentSystem.Data.Seed.Seeds
{
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;

    using StudentSystem.Data.Models.Users;

    using static StudentSystem.Common.Constants.GlobalConstants;

    public class UsersSeed : BaseSeeder<ApplicationUser>
    {
        public UsersSeed(IServiceScope serviceScope, string jsonData) 
            : base(serviceScope, jsonData)
        {
        }

        public async override Task SeedAsync()
        {
            if (await this.IsAlreadySeedAsync())
            {
                return;
            }

            var users = JsonConvert.DeserializeObject<ApplicationUser[]>(this.JsonData);

            await this.DbContext.Users.AddRangeAsync(users);

            await this.DbContext.SaveChangesAsync();

            await this.UserManager.AddPasswordAsync(users[0], AdminPassword);
            await this.UserManager.AddToRoleAsync(users[0], AdminRole);

            foreach (var user in users.Skip(1))
            {
                await this.UserManager.AddPasswordAsync(user, TeacherPassword);
                await this.UserManager.AddToRoleAsync(user, TeacherRole);
            }
        }
    }
}
