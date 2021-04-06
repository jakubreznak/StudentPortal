using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IFileService
    {
         Task<RawUploadResult> AddFileAsync(IFormFile file, string tag);
         Task<DelResResult> RemoveFileAsync(string publicID);
         Task<ArchiveResult> GenerateArchiveURLAsync(string tag);
    }
}