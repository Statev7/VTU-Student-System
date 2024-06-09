namespace StudentSystem.Services.Data.Infrastructure.Services.Contracts
{
    using Microsoft.AspNetCore.Http;
    using StudentSystem.Services.Data.Infrastructure.DTOs.ServiceModels;

    public interface IFileService
    {
        Result<FileServiceModel> GetFile(string path, string name, string extension);

        Task<Result<string>> UploadFileAsync(IFormFile file, string name, string folder);

        void DeleteFromFileSystem(string filePath);
    }
}
