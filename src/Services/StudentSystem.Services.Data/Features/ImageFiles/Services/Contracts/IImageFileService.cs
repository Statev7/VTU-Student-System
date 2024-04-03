namespace StudentSystem.Services.Data.Features.ImageFiles.Services.Contracts
{
    using Microsoft.AspNetCore.Http;

    public interface IImageFileService
    {
        Task<Guid> CreateAsync(IFormFile imageFile, string folder, int resizeWidth);

        void DeleteFromFileSystem(Guid id, string folder);
    }
}
