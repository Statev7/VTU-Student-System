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

            var users = JsonConvert.DeserializeObject<ApplicationUser>(this.JsonData);

            await this.DbSet.AddAsync(users);

            await this.DbContext.SaveChangesAsync();

            await this.UserManager.AddPasswordAsync(users, AdminPassword);
            await this.UserManager.AddToRoleAsync(users, AdminRole);
        }
    }
}
