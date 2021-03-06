# Start and destroy Azure Container Instance groups from a C# Azure Function and with the Azure Management Fluent API.
This sample demonstrated how to create and destroy Azure Container Instance groups from a C# Azure Function by using the Azure Management Fluent API.

It assumes the Azure Function is triggered by a message sent to an Azure Service Bus topic, and the function posts back a response to the same topic containing the resource id of the ACI group created. But the trigger can be changed to anything like HTTP, schedule, Event Grid, or any other trigger supported by Azure Functions.

## Create an Azure Service Principal
You need to create an Azure AD Service Principal, so it can be used by the Azure Function to create Azure Container Instance groups in a resource group of your choice.

`az ad sp create-for-rbac --name <NAME> --role contributor --scopes <RG_RESOURCE_ID> --sdk-auth`
  
Replace NAME tag with the name you want to give to the Azure App and RG_RESOURCE_ID tag with the resource group ID where the Azure Function will deploy Azure Container Instance groups.
  
The command will return a JSON formatted value. Copy and paste it somewhere, we'll need it later.

## Clone this repo
  
`git clone https://github.com/Katalyse/af-aci.git`

## App settings required
You need to set the following app settings in you local.settings.json file for test purposes or directly in your Function App settings.

- ServiceBusConnectionString: Connection string to the Service Bus used by the Azure Function trigger;
- ServicePrincipalClientId: Client ID returned by the az ad sp create-for-rbac command above;
- ServicePrincipalClientSecret: Client Secret returned by the same command;
- ServicePrincipalTenantId: Tenant ID returned by the same command;
- ContainerNamePrefix: Prefix used in the Container Instance groups name;
- ResourceGroupName: Resource Group name where the Container Instance groups will be deployed;
- ContainerRegistryServer: Azure Container Registry server where your container image is stored;
- ContainerRegistryUsername: Azure Container Registry username;
- ContainerRegistryPassword: Azure Container Registry password (you need to enable Admin access on your ACR service).

## Message Bus message examples
The function accepts two types of message:
- START_CONTAINER;
- DELETE_CONTAINER.

**Message examples:**

`
{
  "messageType": "START_CONTAINER"
}
`

`
{
  "messageType": "DELETE_CONTAINER",
  "payload": "/subscriptions/your_subscription_id/resourceGroups/your_resourcegroup_name/providers/Microsoft.ContainerInstance/containerGroups/your_containerinstance_group_name"
}
`
