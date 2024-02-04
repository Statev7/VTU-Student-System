namespace StudentSystem.Data.Seed.Seeds
{
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using Microsoft.Extensions.DependencyInjection;

    using StudentSystem.Data.Models.Users;

    public class CitiesSeed : BaseSeeder<City>
    {
        public CitiesSeed(IServiceScope serviceScope, string jsonData) 
            : base(serviceScope, jsonData)
        {

        }

        public override async Task SeedAsync()
        {
            if(await this.IsAlreadySeedAsync()) 
            {
                return;
            }

            var cities = JsonConvert.DeserializeObject<IEnumerable<City>>(this.JsonData);

            await this.DbSet.AddRangeAsync(cities);

            await this.DbContext.SaveChangesAsync();
        }
    }
}
