﻿using Azure.Storage.Blobs;
using Core.Exceptions;
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

        public BlobStorageService(IConfiguration config)
        {
            _connectionString = config["AzureBlobStorage:ConnectionString"]!;
            _profileContainer = config["AzureBlobStorage:ProfileContainer"]!;
            _postsContainer = config["AzureBlobStorage:PostsContainer"]!;
            _baseUrl = config["AzureBlobStorage:BaseUrl"]!;
        }

        public async Task<string> UpdateProfilePictureByUserId(Guid userId, IFormFile image)
        {
            string extension = Path.GetExtension(image.FileName);

            if (extension != ".jpg" && extension != ".jpeg" && extension != ".png")
            {
                throw new UnsupportedFileTypeException("Unsupported file type for profile picture. (use .jpg .jpeg .png)");
            }

            string blobName = userId.ToString() + extension;

            var blobClient = new BlobClient(_connectionString, _profileContainer, blobName);

            using var stream = image.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return _baseUrl + blobName;
        }
    }
}