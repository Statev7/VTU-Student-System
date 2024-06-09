namespace StudentSystem.Services.Data.Infrastructure.Services.Implementation
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;

    using StudentSystem.Common;
    using StudentSystem.Common.Constants;
    using StudentSystem.Services.Data.Infrastructure.DTOs.ServiceModels;
    using StudentSystem.Services.Data.Infrastructure.Helpers.Contracts;
    using StudentSystem.Services.Data.Infrastructure.Services.Contracts;

    using VirusTotalNet;
    using VirusTotalNet.ResponseCodes;

    using static StudentSystem.Common.Constants.NotificationConstants;
    using static StudentSystem.Services.Data.Infrastructure.Constants.FilesConstants;

    public class FileService : IFileService
    {
        private readonly ApplicationSettings applicationSettings;
        private readonly IFilesHelper filesHelper;

        public FileService(IOptions<ApplicationSettings> options, IFilesHelper filesHelper)
        {
            this.applicationSettings = options.Value;
            this.filesHelper = filesHelper;
        }

        public Result<FileServiceModel> GetFile(string path, string name, string extension)
        {
            var storagePath = this.filesHelper.GetStoragePath(path);

            if (!File.Exists(storagePath))
            {
                return Result<FileServiceModel>.Failure(ErrorMesage);
            }

            var mimeType = MimeTypesConstants.GetMimeType(extension);

            if (string.IsNullOrEmpty(mimeType))
            {
                return Result<FileServiceModel>.Failure(ErrorMesage);
            }

            var nameWithExtension = $"{name}{extension}";

            var fileModel = new FileServiceModel()
            {
                Name = nameWithExtension,
                Path = path,
                ContentType = mimeType,
            };

            return Result<FileServiceModel>.SuccessWith(fileModel);
        }

        public async Task<Result<string>> UploadFileAsync(IFormFile file, string name, string folder)
        {
            var result = await this.CheckFileAsync(file);

            if (!result.Succeed) 
            {
                return Result<string>.Failure(result.Message);
            }

            var filePath = $"/{RootFilesFolderName}/{folder}";

            var storagePath = this.filesHelper.GetStoragePath(filePath);
            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }

            var extension = Path.GetExtension(file.FileName);
            var nameWithExtension = $"{name}{extension}";

            var pathWithFileName = Path.Combine(storagePath, nameWithExtension);

            using FileStream fileStream = File.Create(pathWithFileName);

            await file.CopyToAsync(fileStream);

            var pathForDb = $"{filePath}/{nameWithExtension}";

            return Result<string>.SuccessWith(pathForDb);
        }

        public void DeleteFromFileSystem(string filePath)
            => this.filesHelper.DeleteFromFileSystem(filePath);

        #region Private Methods

        private async Task<Result> CheckFileAsync(IFormFile file)
        {
            if (file.Length <= 0)
            {
                return EmptyFileErrorMessage;
            }

            var virusTotal = new VirusTotal(this.applicationSettings.VirusTotaApiKey);

            using var stream = file.OpenReadStream();
            var fileReport = await virusTotal.GetFileReportAsync(stream);

            while (fileReport.ResponseCode == FileReportResponseCode.Queued)
            {
                Thread.Sleep(1000);
            }

            if (fileReport.Positives != 0)
            {
                return PotentialVirusInFileErrorMessage;
            }

            return true;
        }

        #endregion
    }
}
