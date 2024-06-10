namespace StudentSystem.Services.Data.Infrastructure.Helpers.Contracts
{
    public interface IFilesHelper
    {
        void DeleteFromFileSystem(string filePath);

        string GetStoragePath(string path);
    }
}
