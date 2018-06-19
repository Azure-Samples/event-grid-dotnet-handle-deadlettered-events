
using System;
using Newtonsoft.Json;

namespace DeadLetterSample
{
    //
    // Summary:
    //     Properties of a Dead Letter Event
    public class DeadLetterEvent
    {
        //
        // Summary:
        //     Initializes a new instance of the DeadLetterEvent class.
        public DeadLetterEvent() { }
        
        //
        // Summary:
        //     Gets or sets an unique identifier for the event.
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        
        //
        // Summary:
        //     Gets or sets the resource path of the event source.
        [JsonProperty(PropertyName = "topic")]
        public string Topic { get; set; }
        
        //
        // Summary:
        //     Gets or sets a resource path relative to the topic path.
        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }
        
        //
        // Summary:
        //     Gets or sets event data specific to the event type.
        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }
        
        //
        // Summary:
        //     Gets or sets the type of the event that occurred.
        [JsonProperty(PropertyName = "eventType")]
        public string EventType { get; set; }
        
        //
        // Summary:
        //     Gets or sets the time (in UTC) the event was generated.
        [JsonProperty(PropertyName = "eventTime")]
        public DateTime EventTime { get; set; }
        
        //
        // Summary:
        //     Gets the schema version of the event metadata.
        [JsonProperty(PropertyName = "metadataVersion")]
        public string MetadataVersion { get; }
        
        //
        // Summary:
        //     Gets or sets the schema version of the data object.
        [JsonProperty(PropertyName = "dataVersion")]
        public string DataVersion { get; set; }

        // Below are the dead letter specific fields
        //
        // Summary:
        //     Gets or sets the deadLetterReason of the data object.
        [JsonProperty(PropertyName = "deadLetterReason")]
        public string DeadLetterReason { get; set; }

        //
        // Summary:
        //     Gets or sets the deliveryAttempts of the data object.
        [JsonProperty(PropertyName = "deliveryAttempts")]
        public int DeliveryAttempts { get; set; }

        //
        // Summary:
        //     Gets or sets the lastDeliveryOutcome of the data object.
        [JsonProperty(PropertyName = "lastDeliveryOutcome")]
        public string LastDeliveryOutcome { get; set; }

        //
        // Summary:
        //     Gets or sets the lastHttpStatusCode of the data object.
        [JsonProperty(PropertyName = "lastHttpStatusCode")]
        public int LastHttpStatusCode { get; set; }


    }
}

