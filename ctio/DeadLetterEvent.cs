
using System;
using Newtonsoft.Json;

//
// Summary:
//     Properties of an event published to an Event Grid topic.
public class DeadLetterEvent  
{
    //
    // Summary:
    //     Initializes a new instance of the EventGridEvent class.
    public DeadLetterEvent() { }
    //
    // Summary:
    //     Initializes a new instance of the EventGridEvent class.
    //
    // Parameters:
    //   id:
    //     An unique identifier for the event.
    //
    //   subject:
    //     A resource path relative to the topic path.
    //
    //   data:
    //     Event data specific to the event type.
    //
    //   eventType:
    //     The type of the event that occurred.
    //
    //   eventTime:
    //     The time (in UTC) the event was generated.
    //
    //   dataVersion:
    //     The schema version of the data object.
    //
    //   topic:
    //     The resource path of the event source.
    //
    //   metadataVersion:
    //     The schema version of the event metadata.
    //
    //   deadLetterReason:
    //     The reason behind the message being dead lettered.
    //         
    //   deliveryAttempts:
    //     The total number of delivery attempts
    // 
    //  lastDeliveryOutcome
    //     The outcome of the last delivery attempt.
    //
    //   lastHttpStatusCode:
    //     The response http status code when the last delivery attempt was made.
    //
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

    //Below are the dead letter specific fields

    //
    // Summary:
    //     Gets or sets the deadLetterReason of the data object.
    [JsonProperty(PropertyName = "deadLetterReason")]
    public string DeadLetterReason { get; set; }

    //
    // Summary:
    //     Gets or sets the deliveryAttempts of the data object.
    [JsonProperty(PropertyName = "deliveryAttempts")]
    public string DeliveryAttempts { get; set; }

    //
    // Summary:
    //     Gets or sets the lastDeliveryOutcome of the data object.
    [JsonProperty(PropertyName = "lastDeliveryOutcome")]
    public string LastDeliveryOutcome { get; set; }

    //
    // Summary:
    //     Gets or sets the lastHttpStatusCode of the data object.
    [JsonProperty(PropertyName = "lastHttpStatusCode")]
    public string LastHttpStatusCode { get; set; }


}

