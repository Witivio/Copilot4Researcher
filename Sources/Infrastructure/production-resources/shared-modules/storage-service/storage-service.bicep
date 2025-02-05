
param muiName string
param location string
param storageAccountName string
param blobContainerNames array = []
param vnetName string
// Resources

resource mui 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' existing = {
  name: muiName
}

resource vnet 'Microsoft.Network/virtualNetworks@2023-05-01' existing = {
  name: vnetName
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: storageAccountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  identity:{
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${mui.id}': {}
    }
  }
  properties: {
    supportsHttpsTrafficOnly: true
    allowBlobPublicAccess: true
    minimumTlsVersion: 'TLS1_2'
    networkAcls: {
      bypass: 'AzureServices'
      virtualNetworkRules: []
      ipRules: []
      defaultAction: 'Deny'
    }
  }
}

resource privateEndpoint 'Microsoft.Network/privateEndpoints@2023-02-01' = {
  name: 'storagePrivateEndpoint'
  location: location
  properties: {
    subnet: {
      id: vnet.properties.subnets[1].id
    }
    privateLinkServiceConnections: [
      {
        name: 'storageConnection'
        properties: {
          privateLinkServiceId: storageAccount.id
          groupIds: [
            'blob' // ou 'file', selon le type d'accès nécessaire
          ]
          requestMessage: 'Private Endpoint for Storage Account'
        }
      }
    ]
  }
}

resource privateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' = {
  name: 'privatelink.blob.core.windows.net'
  location: 'global'
  properties: {}
}

resource dnsLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = {
  name: 'dnsLinkToVnet'
  location: 'global'
  parent: privateDnsZone
  properties: {
    virtualNetwork: {
      id: vnet.id
    }
    registrationEnabled: false
  }
}

resource privateDnsZoneGroup 'Microsoft.Network/privateEndpoints/privateDnsZoneGroups@2023-02-01' = {
  name: 'storageDnsZoneGroup'
  parent: privateEndpoint
  properties: {
    privateDnsZoneConfigs: [
      {
        name: 'dnsConfig'
        properties: {
          privateDnsZoneId: privateDnsZone.id
        }
      }
    ]
  }
}

resource storageAccountBlob 'Microsoft.Storage/storageAccounts/blobServices@2023-01-01' = {
  parent: storageAccount
  name: 'default'
  properties: {
    isVersioningEnabled:true
    deleteRetentionPolicy: {
      enabled: true
      days: 30
    }
    containerDeleteRetentionPolicy: {
      enabled: true
      days: 30
    }
  }
}


resource storageAccountBlobContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-01-01' = [for blobContainerName in blobContainerNames: {
  parent: storageAccountBlob
  name: blobContainerName
  properties: {
    publicAccess: 'Blob'
  }
}]

var accountKey = storageAccount.listKeys().keys[0].value

output storageAccountName string = storageAccount.name
output storageAccountBlobContainerNames array = [for blobContainerName in blobContainerNames: blobContainerName]
output connectionString string = 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${accountKey};EndpointSuffix=core.windows.net'
