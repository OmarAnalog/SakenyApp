using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Sakenny.Services.ImageService
{
    public class ImageService
    {
        private readonly Cloudinary cloudinary;

        public ImageService(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder="my_app_images"
            };
            var res=await cloudinary.UploadAsync(uploadParams);
            return res.SecureUrl.ToString();
        }
        public async Task<bool> RemoveImageAsync(string file) {
            var url = ExtractPublicIdFromUrl(file);
            var deleteParams = new DeletionParams(url)
            {
                ResourceType = ResourceType.Image
            };
            var result = await cloudinary.DestroyAsync(deleteParams);
            return result.Result == "ok";
        }
        public string ExtractPublicIdFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;

            var uri = new Uri(url);
            var segments = uri.Segments;
            var uploadIndex = Array.IndexOf(segments, "upload/") + 1;

            if (uploadIndex > 0 && uploadIndex < segments.Length)
            {
                return Path.GetFileNameWithoutExtension(
                    string.Concat(segments.Skip(uploadIndex)));
            }
            return null;
        }

    }
}
