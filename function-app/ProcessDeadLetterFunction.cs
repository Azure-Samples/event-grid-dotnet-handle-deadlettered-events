// This is the default URL for triggering event grid function in the local environment.
// http://localhost:7071/admin/extensions/EventGridExtensionConfig?functionName={functionname} 

using System;
using System.Net;
using System.IO;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeadLetterSample
{
    public static class ProcessDeadLetterFunction
    {
        [FunctionName("ProcessDeadLetter")]
        public static void Run([EventGridTrigger]JObject eventGridEvent, TraceWriter log)
        {
            log.Info($"C# Event grid trigger function has begun...");
            const string StorageBlobCreatedEvent = "Microsoft.Storage.BlobCreated";


            log.Info(eventGridEvent.ToString(Formatting.Indented));
            var egEvent = eventGridEvent.ToObject<Microsoft.Azure.EventGrid.Models.EventGridEvent>();
            JObject dataObject = egEvent.Data as JObject;

            if (string.Equals(egEvent.EventType, StorageBlobCreatedEvent, StringComparison.OrdinalIgnoreCase))
            {
                log.Info("Received blob created event..");

                var data = dataObject.ToObject<StorageBlobCreatedEventData>();
                log.Info($"Dead Letter Blob Url:{data.Url}");

                log.Info("Reading blob data from storage account..");
                ProcessBlob(data.Url, log);
            }
        }

        /*
        This function uses the blob url/location received in the BlobCreated event to fetch data from the storage container.
        Here, we perform a simple GET request on the blob url and deserialize the dead lettered events in a json array.
        */
        public static void ProcessBlob(string url, TraceWriter log)
        {

            // sas key generated through the portal for your storage account used for authentication
            const string sasKey = "--replace-with-the-storage-account-sas-key";
            string uri = url + sasKey;

            WebClient client = new WebClient();

            Stream data = client.OpenRead(uri);
            StreamReader reader = new StreamReader(data);
            string blob = reader.ReadToEnd();
            data.Close();
            reader.Close();

            log.Info($"Dead Letter Events:{blob}");

            // deserialize the blob into dead letter events 
            DeadLetterEvent[] dlEvents = JsonConvert.DeserializeObject<DeadLetterEvent[]>(blob);

            foreach (DeadLetterEvent dlEvent in dlEvents) {
                log.Info($"Printing Dead Letter Event attributes for EventId: {dlEvent.Id}, Dead Letter Reason:{dlEvent.DeadLetterReason}, DeliveryAttempts:{dlEvent.DeliveryAttempts}, LastDeliveryOutcome:{dlEvent.LastDeliveryOutcome}, LastHttpStatusCode:{dlEvent.LastHttpStatusCode}");
                // TODO: steps for processing the dead letter event further
            }
        }
    }
}
