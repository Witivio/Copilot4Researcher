param location string = resourceGroup().location
@description('The name of the service to use for the open ai.')
param serviceName string
param vnetName string

var privateDnsZoneName = 'privatelink.openai.azure.com'


resource vnet 'Microsoft.Network/virtualNetworks@2023-05-01' existing = {
  name: vnetName
}

resource openai 'Microsoft.CognitiveServices/accounts@2023-05-01' = {
  name: serviceName
  location: location
  sku: {
    name: 'S0'
  }
  kind: 'OpenAI'
  properties: {
    apiProperties: {
      enablePrivateEndpoint: true
    }
    customSubDomainName: 'cfr-openai'
    networkAcls: {
      defaultAction: 'Deny'
      virtualNetworkRules: []
      ipRules: []
    }
    publicNetworkAccess: 'Disabled'
  }
 
}

resource openaideployment 'Microsoft.CognitiveServices/accounts/deployments@2023-05-01' = {
  name: 'gpt4-o'
  sku: {
    name: 'GlobalStandard'
    capacity: 1
  }
  parent: openai
  properties: {
    model: {
      name: 'gpt-4o'
      format: 'OpenAI'
      version: '2024-08-06'
    }
    raiPolicyName: 'Microsoft.Default'
    versionUpgradeOption: 'OnceCurrentVersionExpired'
 
  }
}

resource privateEndpoint 'Microsoft.Network/privateEndpoints@2023-02-01' = {
  name: 'openApiPrivateEndpoint'
  location: location
  properties: {
    
    subnet: {
      id: vnet.properties.subnets[1].id
    }
    privateLinkServiceConnections: [
      {
        name: 'openApiConnection'
        properties: {
          privateLinkServiceId: openai.id
          groupIds: [
            'account'
          ]
          requestMessage: 'Private Endpoint for openApi Account'
        }
      }
    ]
  }
}

resource privateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' = {
  name: 'privatelink.openapi.core.windows.net'
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
  name: 'openApiDnsZoneGroup'
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

