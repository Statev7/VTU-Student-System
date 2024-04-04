namespace StudentSystem.Services.Data.Features.ImageFiles.Services.Contracts
{
    using Microsoft.AspNetCore.Http;

    public interface IImageFileService
    {
        Task<string> CreateToFileSystemAsync(IFormFile imageFile, string folder, int resizeWidth);

        void DeleteFromFileSystem(string path);
    }
}
