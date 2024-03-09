namespace StudentSystem.Data.Seed.Seeds
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;

    using StudentSystem.Data.Models.Users;

    public class RoleSeeder : BaseSeeder<ApplicationRole>
    {
        public RoleSeeder(IServiceScope serviceScope, string jsonData) 
            : base(serviceScope, jsonData)
        {

        }

        public override async Task SeedAsync()
        {
            if(await this.IsAlreadySeedAsync())
            {
                return;
            }

            var roles = JsonConvert.DeserializeObject<IEnumerable<ApplicationRole>>(this.JsonData);

            await this.DbContext.Roles.AddRangeAsync(roles);
            await this.DbContext.SaveChangesAsync();
        }
    }
}
