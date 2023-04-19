using Imagekit;
using Imagekit.Models;
using Imagekit.Sdk;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace CapstoneProject_BE.Helper
{
    public class ImageHelper
    {
        private readonly ImagekitClient _client;

        public ImageHelper(string publicKey, string privateKey, string urlEndpoint)
        {
            _client = new ImagekitClient(publicKey, privateKey, urlEndpoint);
        }

        public async Task<string> UploadImageAsync(IFormFile file, string fileName)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                byte[] bytes = memoryStream.ToArray();
                var uploadRequest = new FileCreateRequest()
                {
                    file = bytes,
                    fileName = fileName,
                    useUniqueFileName=true
                };

                var uploadResponse = await _client.UploadAsync(uploadRequest);
                return uploadResponse.url;
            }
        }
    }
}
