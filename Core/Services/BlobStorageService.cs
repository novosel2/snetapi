﻿using Azure.Storage.Blobs;
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
        private readonly string _connectionString;
        private readonly string _profileContainer;
        private readonly string _postsContainer;
        private readonly string _baseUrl;
        private readonly string _defaultPicture;

        public BlobStorageService(IConfiguration config)
        {
            _connectionString = config["AzureBlobStorage:ConnectionString"]!;
            _profileContainer = config["AzureBlobStorage:ProfileContainer"]!;
            _postsContainer = config["AzureBlobStorage:PostsContainer"]!;
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

            byte[] imageBytes = await ConvertToByteArrayAsync(image);
            byte[] processedImage = ImageHelper.ProcessImage(imageBytes, width: 1024, height: 1024, quality: 80);

            using var stream = new MemoryStream(processedImage);
            await blobClient.UploadAsync(stream, overwrite: true);

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
            string blobName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var blobClient = new BlobClient(_connectionString, _postsContainer, blobName);

            byte[] imageBytes = await ConvertToByteArrayAsync(file);
            byte[] processedImage = ImageHelper.ProcessImage(imageBytes, width: 1024, height: 1024, quality: 90);

            using var stream = new MemoryStream(processedImage);
            await blobClient.UploadAsync(stream, overwrite: true);

            return _baseUrl + _postsContainer + $"/{blobName}";
        }

        // Delete post file from blob storage
        public async Task DeletePostFile(string url)
        {
            string blobName = url.Substring(url.LastIndexOf("/") + 1);

            var blobClient = new BlobClient(_connectionString, _postsContainer, blobName);

            await blobClient.DeleteIfExistsAsync();
        }


        private static async Task<byte[]> ConvertToByteArrayAsync(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
