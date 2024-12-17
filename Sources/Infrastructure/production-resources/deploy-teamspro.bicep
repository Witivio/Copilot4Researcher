@description('The deployment environment')
@allowed(['test', 'int', 'prd'])
param deploymentEnvironment string

@description('The serp api key')
@secure()
param SerpApiApiKey string

// GENERAL PARAMETERS
@description('The Api Key pubmed 2')
@secure()
param PubmedApiKeys2 string

@description('The Api Key pubmed 1')
@secure()
param PubmedApiKeys1 string

@description('The Api Key pubmed 0')
@secure()
param PubmedApiKeys0 string

@description('The azure tenant id')
param AzureTenantId string

@description('The azure client id')
param AzureClientId string

// DATABASE POSTGRES
@description('The azure client secret')
@secure()
param AzureClientSecret string


@description('The sharepoint url')
param SharePointUrl string

// ADMIN CENTER APP PARAMETERS
@description('The Sharepoint list id')
param SharePointListId string

@description('The SharePointDeliveryNotesUrl')
param SharePointDeliveryNotesUrl string

@description('SharePointLogListId')
param SharePointLogListId string

@description('The SharePointScanIntervalMinutes')
param SharePointScanIntervalMinutes string

@description('AzureOpenAIChatDeploymentName')
param AzureOpenAIChatDeploymentName string

@description('The AzureOpenAIEndpoint')
param AzureOpenAIEndpoint string

// CAMPAIGN APP PARAMETERS
@description('The application Id from Azure AD for campaign')
@secure()
param AzureOpenAIApiKey string

@secure()
@description('The application secret from Azure AD for campaign')
param ConnectionStrings string

// DATABASE POSTGRES
@description('The admin login for the postgres database')
param postgresAdminUsername string

@secure()
@description('The admin password for the postgres database')
param postgresAdminPassword string



// params with default values
@allowed(['B1', 'P0v3', 'P1v3', 'P2v3'])
param appServicePlanTier string = 'P0v3'

param appPrefix string = 'cfr'
param location string = resourceGroup().location


// Variables
var appServicePlanName = '${appPrefix}-plan-${deploymentEnvironment}'
var KeyVaultName = '${appPrefix}-kv-${deploymentEnvironment}'
var logWorkspaceName = '${appPrefix}-wokspace-${deploymentEnvironment}'

// var frontDoorName = '${appPrefix}-fd-${deploymentEnvironment}'
var vnetName = '${appPrefix}-vnet-${deploymentEnvironment}'

var postgresqlServerName = '${appPrefix}-postgres-${deploymentEnvironment}'
var privateDnsZoneName = 'privatelink.vaultcore.azure.net'
var storageAccountName = '${appPrefix}store${deploymentEnvironment}'

// Roles Keyvault
var kvRoleIdMapping = {
  'Key Vault Administrator': '00482a5a-887f-4fb3-b363-3b7fe8e74483'
  'Key Vault Certificates Officer': 'a4417e6f-fecd-4de8-b567-7b0420556985'
  'Key Vault Crypto Officer': '14b46e9e-c2b7-41b4-b07b-48a6ebf60603'
  'Key Vault Crypto Service Encryption User': 'e147488a-f6f5-4113-8e2d-b22465e65bf6'
  'Key Vault Crypto User': '12338af0-0e69-4776-bea7-57ae8d297424'
  'Key Vault Reader': '21090545-7ca7-4776-b22c-e363652d74d2'
  'Key Vault Secrets Officer': 'b86a8fe4-44ce-4948-aee5-eccb2c155cd7'
  'Key Vault Secrets User': '4633458b-17de-408a-b874-0445c86b69e6'
}

resource mui 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: '${appPrefix}-mui-${deploymentEnvironment}'
  location: location
}

resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: appServicePlanName
  location: location
  sku:{
    name: appServicePlanTier
    tier: 'Standard'
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource vnet 'microsoft.network/virtualNetworks@2021-02-01' = {
  name: vnetName
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        '10.0.0.0/16'
      ]
    }
    subnets:[
      {
        name: 'cfr-appservice-snet'
        properties: {
          addressPrefix: '10.0.1.0/24'
          privateEndpointNetworkPolicies: 'Enabled'
          privateLinkServiceNetworkPolicies: 'Enabled'
          serviceEndpoints: [
            {
              service: 'Microsoft.Web'
              locations: [
                '*'
              ]
            }
          ]
          delegations: [
            {
              name: 'Microsoft.Web/serverFarms'
              properties: {
                serviceName: 'Microsoft.Web/serverFarms'

              }
            }
          ]
        }
      }
      {
        name: 'cfr-keyvault-snet'
        properties: {
          addressPrefix: '10.0.3.0/24'
          privateEndpointNetworkPolicies: 'Enabled'
          privateLinkServiceNetworkPolicies: 'Enabled'
          serviceEndpoints: [
            {
              service: 'Microsoft.Storage'
              locations: [
                '*'
              ]
            }
            {
              service: 'Microsoft.CognitiveServices'
              locations: [
                '*'
              ]
            }
            {
              service: 'Microsoft.Keyvault'
              locations: [
                '*'
              ]
            }
          ]
        }
      }
      {
        name: 'cfr-postgres-snet'
        properties: {
          addressPrefix: '10.0.2.0/24'
          privateEndpointNetworkPolicies: 'Enabled'
          privateLinkServiceNetworkPolicies: 'Enabled'
          serviceEndpoints: [
            {
              service: 'Microsoft.Sql'
              locations: [
                '*'
              ]
            }
          ]
          delegations: [
            {
              name: 'Microsoft.DBforPostgreSQL/flexibleServers'
              properties: {
                serviceName: 'Microsoft.DBforPostgreSQL/flexibleServers'
              }
            }
          ]
        }
      }
      {
        name: 'cfr-app-snet'
        properties: {
          addressPrefix: '10.0.4.0/24'
          privateEndpointNetworkPolicies: 'Enabled'
          privateLinkServiceNetworkPolicies: 'Enabled'
          serviceEndpoints: [
            {
              service: 'Microsoft.Web'
              locations: [
                '*'
              ]
            }
          ]
        }
      }
    ]
  }
}


resource keyVault 'Microsoft.Keyvault/vaults@2023-02-01' = {
  name: KeyVaultName
  location: location
  properties: {
    tenantId: subscription().tenantId
    enableRbacAuthorization: true
    sku: {
      name: 'standard'
      family: 'A'
    }
    networkAcls: {
      bypass: 'AzureServices'
      defaultAction: 'Deny'
      virtualNetworkRules: [
        {
          id: vnet.properties.subnets[1].id
        }
      ]
      ipRules: []
    }
    publicNetworkAccess: 'Disabled'
  }
}

module storageAccount './shared-modules/storage-service/storage-service.bicep' = {
  name: storageAccountName
  params: {
    muiName: mui.name
    location: location
    storageAccountName: storageAccountName
    vnetName: vnetName
    blobContainerNames: [
      'admincenter'
    ]
  }
}

resource privateEndpointKeyvault 'Microsoft.Network/privateEndpoints@2023-05-01' = {
  name: '${appPrefix}-privatekeyvault-${deploymentEnvironment}'
  location: location
  properties: {
    subnet: {
      id: vnet.properties.subnets[1].id
    }
    privateLinkServiceConnections: [
      {
        name: '${KeyVaultName}-connection'
        properties: {
          privateLinkServiceId: keyVault.id
          groupIds: [
            'vault'
          ]
        }
      }
    ]
  }
}

resource privateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' = {
  name: privateDnsZoneName
  location: 'global'
  properties: {}
}

resource dnsZoneLink 'Microsoft.Network/privateEndpoints/privateDnsZoneGroups@2023-02-01' = {
  parent: privateEndpointKeyvault
  name: 'privateDnsZoneGroup'
  properties: {
    privateDnsZoneConfigs: [
      {
        name: privateDnsZoneName
        properties: {
          privateDnsZoneId: privateDnsZone.id
        }
      }
    ]
  }
}

resource kvSecretOfficerRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(kvRoleIdMapping['Key Vault Secrets Officer'], mui.id, keyVault.id)
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', kvRoleIdMapping['Key Vault Secrets Officer'])
    principalId: mui.properties.principalId
    principalType: 'ServicePrincipal'
  }
}

resource kvKeyUserRole 'Microsoft.Authorization/roleAssignments@2020-04-01-preview' = {
  name: guid(kvRoleIdMapping['Key Vault Crypto User'], mui.id, keyVault.id)
  scope: keyVault
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', kvRoleIdMapping['Key Vault Crypto User'])
    principalId: mui.properties.principalId
    principalType: 'ServicePrincipal'
  }
}

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2020-08-01' = {
  name: logWorkspaceName
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    features: {
      searchVersion: 1
      legacy: 0
      enableLogAccessUsingOnlyResourcePermissions: true
    }
    workspaceCapping:{
      dailyQuotaGb:2
    }
  }
}

module postgresServer './shared-modules/postgresql/postgresql-server.bicep' = {
  name: 'deploy-cfr-postgres-db'
  params: {
    adminUsername: postgresAdminUsername
    adminPassword: postgresAdminPassword
    location: location
    muiName: mui.name
    serverName: postgresqlServerName
    vnetName: vnetName
    subnetName: 'cfr-postgres-snet'
  }
  
}


// Variables
var cfrDns = 'https://cfr-copilotforresearcher-prd.azurewebsites.net'
var appServiceName = '${appPrefix}-copilotforresearcher-${deploymentEnvironment}'


resource serpApiApiKey 'Microsoft.KeyVault/vaults/secrets@2022-11-01' = {
  parent: keyVault
  name: 'SerpApiApiKey'
  properties: {
    value: SerpApiApiKey
  }
}

resource pubmedApiKeys2 'Microsoft.KeyVault/vaults/secrets@2022-11-01' = {
  parent: keyVault
  name: 'PubmedApiKeys2'
  properties: {
    value: PubmedApiKeys2
  }
}

resource pubmedApiKeys1 'Microsoft.KeyVault/vaults/secrets@2022-11-01' = {
  parent: keyVault
  name: 'PubmedApiKeys1'
  properties: {
    value: PubmedApiKeys1
  }
}

resource pubmedApiKeys0 'Microsoft.KeyVault/vaults/secrets@2022-11-01' = {
  parent: keyVault
  name: 'PubmedApiKeys0'
  properties: {
    value: PubmedApiKeys0
  }
}

resource azureTenantId 'Microsoft.KeyVault/vaults/secrets@2022-11-01' = {
  parent: keyVault
  name: 'Azure--Tenant--Id'
  properties: {
    value: AzureTenantId
  }
}

resource azureClientId 'Microsoft.KeyVault/vaults/secrets@2022-11-01' = {
  parent: keyVault
  name: 'Azure--Client--Id'
  properties: {
    value: AzureClientId
  }
}

resource azureClientSecret 'Microsoft.KeyVault/vaults/secrets@2022-11-01' = {
  parent: keyVault
  name: 'Azure--Client--Secret'
  properties: {
    value: AzureClientSecret
  }
}

resource sharepointListId 'Microsoft.KeyVault/vaults/secrets@2022-11-01' = {
  parent: keyVault
  name: 'SharepointListId'
  properties: {
    value: SharePointListId
  }
}

resource sharepointLogListid 'Microsoft.KeyVault/vaults/secrets@2022-11-01' = {
  parent: keyVault
  name: 'SharePointLogListId'
  properties: {
    value: SharePointLogListId
  }
}

resource azureOpenAIApiKey 'Microsoft.KeyVault/vaults/secrets@2022-11-01' = {
  parent: keyVault
  name: 'AzureOpenAIApiKey'
  properties: {
    value: AzureOpenAIApiKey
  }
}

resource connectionStrings 'Microsoft.KeyVault/vaults/secrets@2022-11-01' = {
  parent: keyVault
  name: 'ConnectionStrings'
  properties: {
    value: ConnectionStrings
  }
}


module appInsights './shared-modules/appInsight.bicep' = {
  name: appServiceName
  params: {
    serviceName: appServiceName
    location: location
    logWorkspaceName: logAnalyticsWorkspace.name
    appServiceHealthCheckUrl: '${cfrDns}/health'
    deploymentEnvironment: deploymentEnvironment
  }
}

module openAI './shared-modules/openAI.bicep' = {
  name: '${appServiceName}-openAI'
  params: {
    serviceName: appServiceName
    location: location
    vnetName: vnetName
  }
}

module apiMngmt './shared-modules/apimngmt.bicep' = {
  name: '${appServiceName}-apiMngmt'
  params: {
    serviceName: appServiceName
    location: location
  }
}

module appService './shared-modules/app-service/app-service.bicep' = {
  name: '${appServiceName}-appservice'
  params: {
    appCommandLine: 'dotnet TeamsPro.CopilotForResearcher.Web.dll'
    appServiceName: appServiceName
    appServicePlanName: appServicePlanName
    deploymentEnvironment: deploymentEnvironment
    location: location
    muiName: mui.name
    vnetName: vnetName
    sharedAppSettings: [
      {
        name: 'SerpApi__ApiKey'
        value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=${serpApiApiKey.name})'
      }
      {
        name: 'PubmedApiKeys__2'
        value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=${pubmedApiKeys2.name})'
      }
      {
        name: 'PubmedApiKeys__0'
        value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=${pubmedApiKeys0.name})'
      }
      {
        name: 'PubmedApiKeys__1'
        value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=${pubmedApiKeys1.name})'
      }
      {
        name: 'AZURE_TENANT_ID'
        value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=${azureTenantId.name})'
      }
      {
        name: 'AZURE_CLIENT_ID'
        value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=${azureClientId.name})'
      }
      {
        name: 'AZURE_CLIENT_SECRET'
        value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=${azureClientSecret.name})'
      }
      {
        name: 'SharePoint__Url'
        value: SharePointUrl
      }
      {
        name: 'SharePoint__ListId'
        value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=${sharepointListId.name})'
      }
      {
        name: 'SharePoint__DeliveryNotesUrl'
        value: SharePointDeliveryNotesUrl
      }
      {
        name: 'SharePoint__LogListId'
        value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=${sharepointLogListid.name})'
      }
      {
        name: 'SharePoint__ScanIntervalMinutes'
        value: SharePointScanIntervalMinutes
      }
      {
        name: 'AzureOpenAI__ChatDeploymentName'
        value: AzureOpenAIChatDeploymentName
      }
      {
        name: 'AzureOpenAI__Endpoint'
        value: AzureOpenAIEndpoint
      }
      {
        name: 'AzureOpenAI__ApiKey'
        value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=${azureOpenAIApiKey.name})'
      }
      {
        name: 'ConnectionStrings__DefaultConnection'
        value: '@Microsoft.KeyVault(VaultName=${keyVault.name};SecretName=${connectionStrings.name})'
      }
    ]
  }
}



