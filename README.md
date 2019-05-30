# ej2-file-manager-azure-aspnet-core-service

This example demonstrates how to utilize the file manager service for Azure blob storage in ASP.NET Core platform.

## Prerequisites

In order to run the service, we need to create the [Azure blob storage account](https://docs.microsoft.com/en-us/azure/storage/common/storage-quickstart-create-account?tabs=azure-portal) and register the Azure storage details like  account name, password and blob name details with in the RegisterAzure method.

```

  RegisterAzure(string accountName, string accountKey, string blobName)

```

## How to run this application?

To run this application, clone the [`ej2-file-manager-azure-aspnet-core-service`](https://github.com/SyncfusionExamples/ej2-file-manager-azure-aspnet-core-service) repository and then navigate to its appropriate path where it has been located in your system.

To do so, open the command prompt and run the below commands one after the other.

```

git clone https://github.com/SyncfusionExamples/ej2-file-manager-azure-aspnet-core-service.git  FileManagerAzureService
cd FileManagerAzureService

```

## Running application

Once cloned, open solution file in visual studio.Then build the project after restoring the nuget packages and run it.
