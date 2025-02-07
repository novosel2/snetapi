using Azure.Storage.Blobs;
using Core.Exceptions;
using Core.Helpers;
using Core.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly string _baseUrl;
        private readonly string _defaultPicture;
        private readonly string _connectionString;
        
        private readonly string _profileContainer;
        private readonly string _postsContainer;
        private readonly string _videosContainer;

        public BlobStorageService(IConfiguration config)
        {
            _connectionString = config["BlobConnection"]!;
            _profileContainer = config["AzureBlobStorage:ProfileContainer"]!;
            _postsContainer = config["AzureBlobStorage:PostsContainer"]!;
            _videosContainer = config["AzureBlobStorage:VideosContainer"]!;
            _baseUrl = config["AzureBlobStorage:BaseUrl"]!;
            _defaultPicture = _baseUrl + _profileContainer + "/default.jpg";
        }

        // Updates a profile picture
        public async Task<string> UpdateProfilePictureByUserId(Guid userId, IFormFile image)
        {
            string extension = Path.GetExtension(image.FileName);

            if (extension != ".jpg" && extension != ".jpeg" && extension != ".png" && extension != ".webp")
            {
                throw new UnsupportedFileTypeException("Unsupported file type for profile picture. (use .jpg .jpeg .png .webp)");
            }

            string blobName = userId.ToString() + extension;

            var blobClient = new BlobClient(_connectionString, _profileContainer, blobName);

            using var outputStream = new MemoryStream();
            await using var inputStream = image.OpenReadStream();

            FileHelper.ProcessImage(inputStream, outputStream, width: 1024, height: 1024, quality: 75);

            outputStream.Seek(0, SeekOrigin.Begin);
            await blobClient.UploadAsync(outputStream, overwrite: true);

            return _baseUrl + _profileContainer + $"/{blobName}";
        }
    
        // Deletes profile picture from user
        public async Task<string> DeleteProfilePictureByUrl(string pictureUrl)
        {
            string blobName = pictureUrl.Substring(pictureUrl.LastIndexOf('/') + 1);

            var blobClient = new BlobClient(_connectionString, _profileContainer, blobName);

            if (new Uri(pictureUrl) != new Uri(_defaultPicture))
            {
                await blobClient.DeleteIfExistsAsync();
            }

            return _defaultPicture;
        }

        // Add post file to blob storage
        public async Task<string> UploadPostFile(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName);
            string blobName = Guid.NewGuid().ToString();

            var supportedImageTypes = new HashSet<string> { ".jpeg", ".jpg", ".png", ".webp" };
            var supportedVideoTypes = new HashSet<string> { ".mp4", ".mov", ".avi", ".wvm", ".avchd", ".webm", ".flv" };

            if (supportedImageTypes.Contains(extension))
            {
                blobName += ".jpeg";
                extension = ".jpeg";
            }
            else if (supportedVideoTypes.Contains(extension))
            {
                blobName += ".mp4";
                extension = ".mp4";
            }
            else
            {
                throw new UnsupportedFileTypeException($"Unsupported file type for post. (use .jpg .jpeg .png .webp, .mp4 .mov .avi .wvm .avchd .web .flv). [{extension}]");
            }

            BlobClient blobClient;
            using var outputStream = new MemoryStream();
            await using var inputStream = file.OpenReadStream();

            if (extension == ".jpeg")
            {
                blobClient = new BlobClient(_connectionString, _postsContainer, blobName);
                FileHelper.ProcessImage(inputStream, outputStream, width: 1024, height: 1024, quality: 75);
            }
            else
            {
                throw new BadRequestException("Bad request for file post");
            }

            outputStream.Seek(0, SeekOrigin.Begin);
            await blobClient.UploadAsync(outputStream, overwrite: true);

            if (extension == ".jpeg")
                return _baseUrl + _postsContainer + $"/{blobName}";

            return _baseUrl + _videosContainer + $"/{blobName}";
        }

        // Delete post file from blob storage
        public async Task DeletePostFile(string url)
        {
            string blobName = url.Substring(url.LastIndexOf("/") + 1);

            var blobClient = new BlobClient(_connectionString, _postsContainer, blobName);

            await blobClient.DeleteIfExistsAsync();
        }
    }
}