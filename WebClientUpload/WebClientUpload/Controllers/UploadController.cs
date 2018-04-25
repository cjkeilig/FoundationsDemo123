using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace WebClientUpload.Controllers
{
    [RoutePrefix("api/upload")]
    public class UploadController : ApiController
    {
        private static readonly string ServerUploadLocation = @"D:\home\site\wwwroot\Temp";
        [Route("file")]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadSingleFile()
        {
            var streamProvider = new MultipartFormDataStreamProvider(ServerUploadLocation);
            var files = await Request.Content.ReadAsMultipartAsync(streamProvider);
            byte[] content = System.IO.File.ReadAllBytes(files.FileData[0].LocalFileName);
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
    CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a queue.
            CloudQueue queue = queueClient.GetQueueReference("YourQueueNameHere"); // REplace with your own

            CloudQueueMessage message = new CloudQueueMessage(content);
            queue.AddMessage(message);
            return Request.CreateResponse(HttpStatusCode.OK);

        }
    }

}
