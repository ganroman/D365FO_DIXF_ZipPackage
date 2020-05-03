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
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;


namespace D365FO_DIXF_ZipPackage
{
    public static class D365FO_DIXF_ZipPackage
    {
        private static CloudBlobDirectory GetArchiveFolder(ILogger log, string azureStorageConnectionString, string BLOBcontainer, string pathToArchive)
        {
            CloudBlobDirectory archiveFolder = null;
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureStorageConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(BLOBcontainer);
                log.LogInformation($"Blob container {container.StorageUri} opened");
 
                archiveFolder = container.GetDirectoryReference(pathToArchive);
            
                log.LogInformation($"Archive folder {archiveFolder.StorageUri} opened");

            }
            catch (Exception ex)
            {
                log.LogInformation(ex, "Exception has been thrown on connecting to storage account");
            }
            return archiveFolder;

        }

        [FunctionName("D365FO_DIXF_ZipPackage")]
       
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "D365FO_DIXF_ZipPackage/{BLOBContainer}/{pathToArchive}")] HttpRequest req,
            string BLOBContainer,
            string pathToArchive,
            ILogger log,
            ExecutionContext context)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var config = new ConfigurationBuilder()
                             .SetBasePath(context.FunctionAppDirectory)
                             .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                             .AddEnvironmentVariables()
                             .Build();


            var azureStorareConnetionString = config["AzureWebJobsStorage"];//config.GetConnectionString("AzureFileStorageConnectionString");

            if (BLOBContainer == null)
            {
                return new BadRequestObjectResult("Please pass BLOB container name on the query string using parameter 'BlobContainer' or in the request body");
            }
            else 
            { 
                log.LogInformation($"Input parameter BLOBContainer: {BLOBContainer}");
            }
            if (pathToArchive == null)
            {
                  return new BadRequestObjectResult("Please pass archive folder name on the query string using parameter 'pathToArchive' or in the request body");
            }
            else
            {
                log.LogInformation($"Input parameter pathToArchive: {pathToArchive}");
            }

            string packageBlobName = "";

            //Reach specified folder in cloud storage container
            var archiveFolder = GetArchiveFolder(log, azureStorareConnetionString, BLOBContainer, pathToArchive);

            if (archiveFolder != null)
            {
                try
                {
                    
                    //Create empty archive
                    var package = archiveFolder.GetBlockBlobReference(archiveFolder.Prefix.Replace("/","") + ".zip");
                    package.UploadText("");

                    log.LogInformation($"Archive file {package.StorageUri} has been created.");

                    //List files in the container, except of package ZIP itself
                    var blobs = archiveFolder.ListBlobs().Cast<CloudBlob>().Where(b => b.Name != package.Name).ToList();     
                    log.LogInformation($"{blobs.Count} blob files listed in {archiveFolder.Prefix}.");

                    using (var stream = await package.OpenWriteAsync())
                    {
                        log.LogInformation($"Archive file {package.Name} has been opened for writing.");
                        using (var zip = new ZipArchive(stream, ZipArchiveMode.Create))
                        {
                            log.LogInformation($"ZipArchive has been created.");
                            //Iterate through files and store them to archive
                            foreach (CloudBlob blob in blobs)
                            {
                                log.LogInformation($"File {blob.Name} is being added to archive opened");
                                using (var fileStream = await blob.OpenReadAsync())
                                {
                                    var entry = zip.CreateEntry(blob.Name.Replace(blob.Parent.Prefix, ""), CompressionLevel.Optimal);
                                    using (var entryStream = entry.Open())
                                    {
                                        await fileStream.CopyToAsync(entryStream);
                                    }
                                }
                                log.LogInformation($"File {blob.Name} has been added to archive opened");
                            }
                        }
                    }
                    //Check size of archive created
                    packageBlobName = package.Name;
                    using (Stream check = package.OpenRead())
                    {
                        log.LogInformation($"Size of {package.Name} is {check.Length}");
                    }
                }
                catch (Exception ex)
                {
                    log.LogInformation(ex, $"Exception has been thrown");
                    //return (ActionResult) new ErrorObjectResult
                }
            }
            else
            {
                return (ActionResult)new NotFoundObjectResult($"Path is not reachable  {BLOBContainer}/{pathToArchive}");
            }

            return (ActionResult)new OkObjectResult($"{packageBlobName}");
        }

    }
}
