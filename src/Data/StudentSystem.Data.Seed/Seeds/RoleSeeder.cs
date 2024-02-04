namespace StudentSystem.Data.Seed.Seeds
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;

    public class RoleSeeder : BaseSeeder<IdentityRole>
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

            var roles = JsonConvert.DeserializeObject<IEnumerable<IdentityRole>>(this.JsonData);

            await this.DbContext.Roles.AddRangeAsync(roles);
            await this.DbContext.SaveChangesAsync();
        }
    }
}
