using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlobStorageDemo.Helpers
{
    public static class StorageHelper
    {
        private static CloudStorageAccount storageAccount;
        private static CloudBlobClient blobClient;
        public static string StorageConnectionString
        {
            set
            {
                if (!CloudStorageAccount.TryParse(value, out storageAccount))
                {
                    throw new StorageException("Invalid connection string, unable to create storage account");
                }
                blobClient = storageAccount.CreateCloudBlobClient();
            }
        }

        private static async Task<CloudBlobContainer> CreateContainerIfNotExistsAsync(string containerName)
        {
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            await container.CreateIfNotExistsAsync();
            var permission = new BlobContainerPermissions()
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };
            await container.SetPermissionsAsync(permission);
            return container;
        }

        public static async Task<string> SaveFileAsync(string filePath, string containerName)
        {
            var container = await CreateContainerIfNotExistsAsync(containerName);
            var fileName = Path.GetFileName(filePath);
            var blob = container.GetBlockBlobReference(fileName);
            await blob.DeleteIfExistsAsync();
            await blob.UploadFromFileAsync(filePath);
            return blob.Uri.AbsoluteUri;
        }
    }
}
