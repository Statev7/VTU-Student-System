namespace StudentSystem.Services.Data.Features.ImageFiles.Services.Implementation
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;

    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Formats.Jpeg;
    using SixLabors.ImageSharp.Processing;

    using StudentSystem.Services.Data.Features.ImageFiles.Services.Contracts;

    public class ImageFileService :IImageFileService
    {
        private const string FilesFolder = "files";

        private readonly IWebHostEnvironment environment;

        public ImageFileService(IWebHostEnvironment environment)
            => this.environment = environment;

        public async Task<string> CreateToFileSystemAsync(IFormFile imageFile, string folder, int resizeWidth)
        {
            using var loadedImage = await Image.LoadAsync(imageFile.OpenReadStream());

            var path = $"/{FilesFolder}/{folder}";
            var name = $"{Guid.NewGuid()}.jpg";

            var storagePath = this.GetStoragePath(path);
            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }

            loadedImage.Metadata.ExifProfile = null;

            if (loadedImage.Width > resizeWidth)
            {
                var resizedHeight = (int)((double) resizeWidth / loadedImage.Width * loadedImage.Height);

                loadedImage.Mutate(options => options.Resize(resizeWidth, resizedHeight));
            }

            await loadedImage.SaveAsJpegAsync($"{storagePath}/{name}", new JpegEncoder() { Quality = 85 });

            return $"{path}/{name}";
        }

        public void DeleteFromFileSystem(string filePath)
        {
            var path = $"/{FilesFolder}/{filePath}";

            var storagePath = this.GetStoragePath(path);

            if (File.Exists(storagePath))
            {
                File.Delete(storagePath);
            }
        }

        private string GetStoragePath(string path)
        {
            var webRootPath = this.environment.WebRootPath;

            var storagePath = Path.Combine(Directory.GetCurrentDirectory(), $"{webRootPath}{path}".Replace("/", "\\"));

            return storagePath;
        }
    }
}
