using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TheAmazingRace
{
    public static class StorageConnectionString
    {
        static string AccountName = System.Configuration.ConfigurationManager.AppSettings["StorageAccountName"];
        static string AccountKey = System.Configuration.ConfigurationManager.AppSettings["StorageAccountKey"];
        static string ContainerName = System.Configuration.ConfigurationManager.AppSettings["StorageContainer"];

        public static void UploadFileToStorage(Stream fileStream, string fileName)
        {
            // Create storagecredentials object by reading the values from the configuration (appsettings.json)
            StorageCredentials storageCredentials = new StorageCredentials(AccountName, AccountKey);

            // Create cloudstorage account by passing the storagecredentials
            CloudStorageAccount storageAccount = new CloudStorageAccount(storageCredentials, true);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Get reference to the blob container by passing the name by reading the value from the configuration (appsettings.json)
            CloudBlobContainer container = blobClient.GetContainerReference(ContainerName);

            // Get the reference to the block blob from the container
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

            // Upload the file
            blockBlob.UploadFromStream(fileStream);
            var result2 = blockBlob.UploadFromStreamAsync(fileStream);
        }
    }
}