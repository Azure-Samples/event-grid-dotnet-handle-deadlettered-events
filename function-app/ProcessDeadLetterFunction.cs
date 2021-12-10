// This is the default URL for triggering event grid function in the local environment.
// http://localhost:7071/admin/extensions/EventGridExtensionConfig?functionName={functionname} 

using System;
using System.Net;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using Microsoft.Extensions.Logging;

namespace DeadLetterSample
{
    public static class ProcessDeadLetterFunction
    {
        [FunctionName("ProcessDeadLetter")]
        public static void Run([EventGridTrigger]JObject eventGridEvent, ILogger logger)
        {
            logger.LogInformation($"C# Event grid trigger function has begun...");
            const string StorageBlobCreatedEvent = "Microsoft.Storage.BlobCreated";


            logger.LogInformation(eventGridEvent.ToString(Formatting.Indented));
            var egEvent = eventGridEvent.ToObject<EventGridEvent>();
            JObject dataObject = egEvent.Data.ToObjectFromJson<JObject>();

            if (string.Equals(egEvent.EventType, StorageBlobCreatedEvent, StringComparison.OrdinalIgnoreCase))
            {
                logger.LogInformation("Received blob created event..");

                var data = dataObject.ToObject<StorageBlobCreatedEventData>();
                logger.LogInformation($"Dead Letter Blob Url:{data.Url}");

                logger.LogInformation("Reading blob data from storage account..");
                ProcessBlob(data.Url, logger);
            }
        }

        /*
        This function uses the blob url/location received in the BlobCreated event to fetch data from the storage container.
        Here, we perform a simple GET request on the blob url and deserialize the dead lettered events in a json array.
        */
        public static void ProcessBlob(string url, ILogger logger)
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

            logger.LogInformation($"Dead Letter Events:{blob}");

            // deserialize the blob into dead letter events 
            DeadLetterEvent[] dlEvents = JsonConvert.DeserializeObject<DeadLetterEvent[]>(blob);

            foreach (DeadLetterEvent dlEvent in dlEvents) {
                logger.LogInformation($"Printing Dead Letter Event attributes for EventId: {dlEvent.Id}, Dead Letter Reason:{dlEvent.DeadLetterReason}, DeliveryAttempts:{dlEvent.DeliveryAttempts}, LastDeliveryOutcome:{dlEvent.LastDeliveryOutcome}, LastHttpStatusCode:{dlEvent.LastHttpStatusCode}");
                // TODO: steps for processing the dead letter event further
            }
        }
    }
}
