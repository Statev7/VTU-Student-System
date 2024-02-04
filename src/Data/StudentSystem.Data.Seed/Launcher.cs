namespace StudentSystem.Data.Seed
{
    using System.Reflection;

    using Microsoft.Extensions.DependencyInjection;

    using StudentSystem.Data.Seed.Contracts;
    using StudentSystem.Data.Seed.Seeds;

    public static class Launcher
    {
        private const string Jsons_Directory = @"Data\StudentSystem.Data.Seed\Jsons";

        public static async Task SeedDataBaseAsync(IServiceProvider serviceProvider)
        {
            if(serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }
            using var serviceScope = serviceProvider.CreateScope();

            var seeders = InitializedSeeds(serviceScope);

            var isDataAlreadySeed = seeders.ToList().TrueForAll(s => s.IsAlreadySeedAsync().GetAwaiter().GetResult());
            if (!isDataAlreadySeed)
            {
                foreach (var seed in seeders)
                {
                    await seed.SeedAsync();
                }
            }
        }

        private static IEnumerable<ISeeder> InitializedSeeds(IServiceScope serviceScope)
            => new List<ISeeder>()
            {
                new RoleSeeder(serviceScope, GetJsonContentAsync(Constants.RolesJson).GetAwaiter().GetResult()),
                new CitiesSeed(serviceScope, GetJsonContentAsync(Constants.CitiesJson).GetAwaiter().GetResult()),
            };

        private static async Task<string> GetJsonContentAsync(string jsonFileName)
        {
            var directory = GetSolutionDirectory();
            var jsonsDirectory = Path.Combine(directory, Jsons_Directory);

            var allSeedJsonFiles = Directory.GetFiles(jsonsDirectory, "*.json", SearchOption.AllDirectories);

            var jsonFile = jsonFileName + ".json";
            var jsonPath = allSeedJsonFiles.SingleOrDefault(path => path.EndsWith(jsonFile));

             var jsonData = await File.ReadAllTextAsync(jsonPath);

            return jsonData;
        }

        private static string GetSolutionDirectory()
        {
            var directory = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;

            while (!Directory.GetFiles(directory, "*.sln").Any())
            {
                directory = Directory.GetParent(directory).FullName;
                if (directory == null)
                {
                    throw new Exception("Solution directory not found.");
                }
            }

            return directory;
        }
    }
}
