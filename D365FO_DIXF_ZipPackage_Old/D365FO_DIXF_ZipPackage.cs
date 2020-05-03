using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System.IO.Compression;

namespace D365FO_DIXF_ZipPackage
{
    public static class D365FO_DIXF_ZipPackage
    {

        private static CloudBlobDirectory GetArchiveFolder(ILogger log, string azureStorageConnectionString, string BLOBcontainer, string storageMainFolderName, string pathToArchive)
        {
            CloudBlobDirectory archiveFolder = null;
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureStorageConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(BLOBcontainer);
                log.LogInformation($"Blob container {BLOBcontainer} opened");

                CloudBlobDirectory dmfFileShare = container.GetDirectoryReference(storageMainFolderName);
                log.LogInformation($"Storage main folder {storageMainFolderName} opened");

                archiveFolder = dmfFileShare.GetDirectoryReference(pathToArchive);
                log.LogInformation($"Archive folder {pathToArchive} opened");

            }
            catch (Exception ex)
            {
                log.LogInformation(String.Format("Exception {0}", ex.Message));
            }
            return archiveFolder;

        }

        private static string FormatFileName(string name, string parentName)
        {
            string fileName = name;
            fileName = fileName.Replace(oldValue: parentName, newValue: "");
            fileName = fileName.Replace("/", "");
            return fileName;
        }

        [FunctionName("D365FO_DIXF_ZipPackage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            ExecutionContext context)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var config = new ConfigurationBuilder()
                             .SetBasePath(context.FunctionAppDirectory)
                             .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                             .AddEnvironmentVariables()
                             .Build();


            var azureFileStorareConnetionString = config["AzureFileStorageConnectionString"];
            //var BLOBcontainer = config["BLOBContainer"];
            //var storageMainFolderName = config["StorageMainFolder"];
            var BLOBcontainer = req.Query["BLOBContainer"];
            var storageMainFolderName = req.Query["StorageMainFolder"];
            string pathToArchive = req.Query["pathToArchive"];

            if (pathToArchive == null)
            {
                return new BadRequestObjectResult("Please pass archive folder name on the query string using parameter 'pathToArchive' or in the request body");
            }

            string packageBlobName = "";

            //Reach specified folder in cloud storage container
            var archiveFolder = GetArchiveFolder(log, azureFileStorareConnetionString, BLOBcontainer, storageMainFolderName, pathToArchive);

            if (archiveFolder != null)
            {
                try
                {
                    //List files in the container
                    var blobs = archiveFolder.ListBlobs();
                    //Create empty archive
                    var package = archiveFolder.GetBlockBlobReference(FormatFileName(archiveFolder.Prefix, archiveFolder.Parent.Prefix) + ".zip");
                    packageBlobName = package.Name;

                    using (var stream = await package.OpenWriteAsync())
                    {
                        using (var zip = new ZipArchive(stream, ZipArchiveMode.Create))
                        {
                            //Iterate throug files and store them to archive
                            foreach (CloudBlob blob in blobs)
                            {
                                log.LogInformation(String.Format("File {0} is being added to archive opened", blob.Name));
                                using (var fileStream = await blob.OpenReadAsync())
                                {
                                    var entry = zip.CreateEntry(FormatFileName(blob.Name, blob.Parent.Prefix), CompressionLevel.Optimal);
                                    using (var entryStream = entry.Open())
                                    {
                                        await fileStream.CopyToAsync(entryStream);
                                    }
                                }
                                log.LogInformation(String.Format("File {0} has been added to archive opened", blob.Name));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.LogInformation($"Exception {ex.Message}");
                    //return (ActionResult) new ErrorObjectResult
                }
            }
            else
            {
                return (ActionResult)new NotFoundObjectResult($"Path is not reachable  {BLOBcontainer}/{storageMainFolderName}/{pathToArchive}");
            }

            return (ActionResult) new OkObjectResult($"Package archive, {packageBlobName}");
        }
    }
}
