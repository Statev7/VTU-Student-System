namespace StudentSystem.Services.Data.Features.ImageFiles.Services.Implementation
{
    using AutoMapper;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;

    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Formats.Jpeg;
    using SixLabors.ImageSharp.Processing;

    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Services.Data.Features.ImageFiles.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Services;

    public class ImageFileService : BaseService<ImageFile>, IImageFileService
    {
        private const string FilesFolder = "files";

        private readonly IWebHostEnvironment environment;

        public ImageFileService(IRepository<ImageFile> repository, IMapper mapper, IWebHostEnvironment environment)
            : base(repository, mapper) 
            => this.environment = environment;

        public async Task<Guid> CreateAsync(IFormFile imageFile, string folder, int resizeWidth)
        {
            using var loadedImage = await Image.LoadAsync(imageFile.OpenReadStream());

            var id = Guid.NewGuid();
            var path = $"/{FilesFolder}/{folder}/";
            var name = $"{id}.jpg";

            var storagePath = this.GetStoragePath(path);
            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }

            loadedImage.Metadata.ExifProfile = null;

            if (loadedImage.Width > resizeWidth)
            {
                var newHeigth = (int)((double) resizeWidth / loadedImage.Width * loadedImage.Height);

                loadedImage.Mutate(options => options.Resize(resizeWidth, newHeigth));
            }

            await loadedImage.SaveAsJpegAsync($"{storagePath}/{name}", new JpegEncoder() { Quality = 85 });

            var imageFileEntity = new ImageFile() { Id = id, Folder = path };

            await this.Repository.AddAsync(imageFileEntity);
            await this.Repository.SaveChangesAsync();

            return id;
        }

        public void DeleteFromFileSystem(Guid id, string folder)
        {
            var path = $"/{FilesFolder}/{folder}/{id}.jpg";

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
