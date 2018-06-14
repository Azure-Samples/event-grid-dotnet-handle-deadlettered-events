


services: event-grid
platforms: dotnet
author: sanariel

---
# Microsoft Azure Event Grid Management Samples for C#

## Contents
1. The ***event-subscription-with-dead-lettering*** project
	- This project contains a sample that demonstrates how to create event subscriptions with dead-letter destinations and 	retry policies.
2. The ***function-app*** project.
	- This project contains:
		- A function sample, ***WebhookSubscriptionFunction*** that acts as a web-hook subscription endpoint for the event subscription.
		- A function sample, ***ProcessDeadLetter*** that processes dead letter events.

## Overview

This sample demonstrates:
- How to create an event subscription with webhook destination, storage-blob dead letter destination and retry policy configuration 
- How to create a function that gets triggered by the storage blobs and process dead letter events.

 We will follow the following steps through this sample:
 1. Create an event grid event subscription with a webhook endpoint, dead letter destination and retry policies options.
	- Create a function which will serve as the web-hook endpoint for the event subscription. The    	*WebhookSubscriptionFunction*  under the function-app handles the 
	   subscription handshake. It returns a 400 Bad Request to all other incoming requests. 
	   *Note*: *the function is programmed in such a way to demonstrate the workings of dead letter destinations.
		A 400 Bad Request will cause the events to end up in the dead letter destination once they expire based on the retry policy.*
	 - Create an Azure Storage Blob Container which will be our dead letter destination for the event subscription.
 2. Demonstrate how to process dead letter events. 
	- Create a function of type event grid trigger which will process dead letter events.



### Prerequisites
- .NET Core 2.0 or higher

### Dependencies
- Event Grid management plane SDK (Microsoft.Azure.Management.EventGrid).

### Installation
- Visual Studio 2017 Version 15.5 or later, with "Azure Development" workload enabled.
- Azure Functions Extension in Visual Studio 2017.
- Clone this repository onto your local machine. Compile the samples inside Visual Studio, the required Microsoft Azure Event Grid SDK components will automatically be downloaded from nuget.org.
 

 ## Running the Sample
 The following are the steps for running this sample end to end:

 1. Create an Event Grid topic: 
	 - You will need to first create an Event Grid topic. 
	 - Follow steps for creating a topic [here](https://docs.microsoft.com/en-us/azure/event-grid/scripts/event-grid-cli-create-custom-topic.). 
	 - Make a note of the topic name and resource group name. 

 2. Create a Storage Blob Container
	  - Create an azure storage blob container as a destination for the dead letter events. 
	  - Follow steps for the same [here.](https://docs.microsoft.com/en-us/azure/storage/common/storage-create-storage-account%5C) 

 3. Creating Azure Functions:
	- Build the ***function-app*** in Visual Studio. Right click on the project in Visual Studio, and click Publish to publish the     	*WebhookSubscriptionFunction* and 
	   *ProcessDeadLetterFunction* to the cloud as an Azure Function. 
	   For more details, please refer to the steps described [here](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs.). 
	- Once you have published this as an Azure function, navigate to the newly published *WebhookSubscriptionFunction* in Azure Portal. Click on "Get Function URL" button for the *WebhookSubscriptionFunction* and copy the function URL.
	-  Navigate to the *ProcessDeadLetter* function in Azure Portal. 
	   Click on the "Add Event Grid subscription" option to create a new event subscription for this event grid trigger function.
	   For the Create Event Subscription form you see, follow the below guidelines:
			Topic Type: Select Storage Accounts as the Topic Type 
			Resource Group: use an existing Resource Group (same as the dead letter storage account in Step 3)
			Instance: You should see the storage account created in step 3 under the Instance dropdown
			SubscriberType: Webhook

		Alternately, you can use the below azure shell commands to create this event susbscription:
		
		```
		#replace values within <> with appropriate settings
		set azure_subscription_id=<azure_subscription_id>
		set storage_account_name=<storage_account_name>
		set resource_group_name=<resource_group_name>
		set function_url=<endpoint> 
		set event_subscription_name=<event_subscription_name>
		
		#setting azure subscription id
		az account set --subscription $azure_subscription_id  
		
		storageid=$(az storage account show --name $storage_account_name --resource-group $resource_group_name --			query id --output tsv)
		endpoint=$endpoint
		
		#create event grid subscription 	
		az eventgrid event-subscription create \
		--resource-id $storageid \
		--name $event_subscription_name \
		--endpoint $endpoint
		```	

		*Note: In the above commands, the <storage_account_name> and <resource_group_name> should be the same as the account created in step 2.  The <function_url> refers to the Url for the ProcessDeadLetter function.*

	- In this step we just created and deployed two functions under the function-app project.	
			- *WebhookSubscriptionFunction* to serve as a webhook endpoint for the subscription we will create in the next step
			- *ProcessDeadLetterFunction* to process dead letter events
	
 4. Create an Event Subscription
	- Create an event subscription to the topic created in step 1 using the ***event-grid-dotnet-dead-lettering*** project.
	- Provide the *WebhookEventSubscription* Azure function URL from step 2. as a web-hook destination endpoint for the subscription. 
	- Provide the Azure Storage Blob ResourceId and Container Name for the dead letter destination.
	- Select appropriate values for the retry policy configuration. 
	- [This](https://docs.microsoft.com/en-us/azure/event-grid/scripts/event-grid-cli-subscribe-custom-topic) further describes how to create an event subscription.

 5. Publish Events:
	- You can use the Event Grid Publisher [project](https://github.com/Azure-Samples/event-grid-dotnet-publish-consume-events/tree/master/EventGridPublisher) to publish events.
 6. Verify Receipt of Events: 
	In this step, we will be verifying that the events are delivered to the event subscription function. Here are the steps:
	- In the Logs view of the *WebhookSubscriptionFunction*, verify that you can see the logs that show the receipt of the EventGridEvent.
	- Since the function responds with a Bad Request to events, you should start seeing those events appear in the dead letter destination.
	- Verify you received the events in the dead letter destination by looking at the blob files under the storage account through the portal.

 7. Process the Dead Letter Events:
	- Look at the Logs view for the *ProcessDeadLetter* function in the portal. You should see the function being triggered by the *BlobCreated* event whenever a new dead letter event is delivered to the blob container/dead letter destination. 
	- Editing the *ProcessDeadLetter* function and republishing should allow you to process the dead letter events. 
	   Note: Look for The ***TODO*** comment in the *ProcessDeadLetter* function to add code to further process the dead letter events.

## Resources

(Any additional resources or related projects)

- https://docs.microsoft.com/en-us/azure/event-grid/overview
- https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs