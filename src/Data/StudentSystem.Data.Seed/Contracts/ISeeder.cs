namespace StudentSystem.Data.Seed.Contracts
{
    public interface ISeeder
    {
        Task SeedAsync();

        Task<bool> IsAlreadySeedAsync();
    }
}
