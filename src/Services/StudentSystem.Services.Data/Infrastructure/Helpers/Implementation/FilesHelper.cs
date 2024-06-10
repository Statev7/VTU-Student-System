namespace StudentSystem.Services.Data.Infrastructure.Helpers.Implementation
{
    using Microsoft.AspNetCore.Hosting;

    using StudentSystem.Services.Data.Infrastructure.Helpers.Contracts;

    public class FilesHelper : IFilesHelper
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public FilesHelper(IWebHostEnvironment webHostEnvironment) 
            => this.webHostEnvironment = webHostEnvironment;

        public void DeleteFromFileSystem(string filePath)
        {
            var storagePath = this.GetStoragePath(filePath);

            if (File.Exists(storagePath))
            {
                File.Delete(storagePath);
            }
        }

        public string GetStoragePath(string path)
        {
            var webRootPath = this.webHostEnvironment.WebRootPath;

            var storagePath = Path.Combine(Directory.GetCurrentDirectory(), $"{webRootPath}{path}".Replace("/", "\\"));

            return storagePath;
        }
    }
}
