namespace StudentSystem.Services.Data.Features.ImageFiles.Services.Implementation
{
    using Microsoft.AspNetCore.Http;

    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Formats.Jpeg;
    using SixLabors.ImageSharp.Processing;

    using StudentSystem.Services.Data.Features.ImageFiles.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure.Helpers.Contracts;

    using static StudentSystem.Services.Data.Infrastructure.Constants.FilesConstants;

    public class ImageFileService :IImageFileService
    {
        private readonly IFilesHelper filesHelper;

        public ImageFileService(IFilesHelper filesHelper) 
            => this.filesHelper = filesHelper;

        public async Task<string> CreateToFileSystemAsync(IFormFile imageFile, string folder, int resizeWidth)
        {
            using var loadedImage = await Image.LoadAsync(imageFile.OpenReadStream());

            var path = $"/{RootFilesFolderName}/{folder}";
            var name = $"{Guid.NewGuid()}.jpg";

            var storagePath = this.filesHelper.GetStoragePath(path);
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
            => this.filesHelper.DeleteFromFileSystem(filePath);
    }
}
