using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IFileService
    {
         Task<RawUploadResult> AddFileAsync(IFormFile file);
         Task<DeletionResult> RemoveFileAsync(string publicID);
    }
}