using System.Threading.Tasks;
using API.HelpClass;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class FileService : IFileService
    {
        private readonly Cloudinary cloudinary;
        public FileService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account
            (
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            this.cloudinary = new Cloudinary(acc);
        }

        public async Task<RawUploadResult> AddFileAsync(IFormFile file)
        {
            var uploadResult = new RawUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new RawUploadParams()
                {
                    File = new FileDescription(file.FileName, stream)
                };
                uploadResult = await this.cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }
    }
}