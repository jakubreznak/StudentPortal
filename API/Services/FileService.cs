using System.Collections.Generic;
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

        public async Task<RawUploadResult> AddFileAsync(IFormFile file, string tag, string cloudinaryFileName)
        {
            var uploadResult = new RawUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new RawUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Tags = tag,
                    PublicId = cloudinaryFileName
                };
                uploadResult = await this.cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<RawUploadResult> AddFileAsync(IFormFile file, string tag)
        {
            var uploadResult = new RawUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new RawUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Tags = tag,
                };
                uploadResult = await this.cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DelResResult> RemoveFileAsync(string publicID)
        {
            List<string> publicIDs = new List<string>();
            publicIDs.Add(publicID);
            var deletionParams = new DelResParams()
            {
                PublicIds = publicIDs,
                ResourceType = ResourceType.Raw
            };
            var result = await this.cloudinary.DeleteResourcesAsync(deletionParams);

            if (result.Deleted[publicID] == "deleted")
                return result;
            else
                deletionParams.ResourceType = ResourceType.Image;

            result = await this.cloudinary.DeleteResourcesAsync(deletionParams);
            if (result.Deleted[publicID] == "deleted")
                return result;
            else
                deletionParams.ResourceType = ResourceType.Video;

            return result = await this.cloudinary.DeleteResourcesAsync(deletionParams);

        }

        public async Task<ArchiveResult> GenerateArchiveURLAsync(string tag, string cloudinaryFileName)
        {
            List<string> tags = new List<string>();
            tags.Add(tag);
            var archiveParams = new ArchiveParams(){};
            archiveParams.ResourceType("all");
            archiveParams = archiveParams.Tags(tags);
            archiveParams.TargetPublicId(cloudinaryFileName);
            var archiveResult = await this.cloudinary.CreateZipAsync(archiveParams);
            return archiveResult;
        }
    }
}