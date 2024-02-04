namespace StudentSystem.Services.Data.Features.City.Services.Contracts
{
    public interface ICityService
    {
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>();
    }
}
