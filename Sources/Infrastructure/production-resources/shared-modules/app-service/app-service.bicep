
param appServiceName string

@allowed(['test', 'int', 'prd'])
param deploymentEnvironment string

param location string
param muiName string
param appServicePlanName string
@description('the shared app settings that will be applied to both production and preproduction slots')
param sharedAppSettings array
@description('the command line that will be used to start the application')
param appCommandLine string
param vnetName string

var productionSlotAppSettingsFull = concat(sharedAppSettings)

var privateEndpointName = 'privateAppServiceEndpoint'

var privateDnsZoneName = 'privatelink.cfr.appservices.net'


resource mui 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' existing = {
  name: muiName
}

resource appServicePlan 'Microsoft.Web/serverfarms@2021-02-01' existing = {
  name: appServicePlanName
}

resource vnet 'Microsoft.Network/virtualNetworks@2023-05-01' existing = {
  name: vnetName
}


resource appService 'Microsoft.Web/sites@2022-09-01' = {
  name:appServiceName
  location: location
  kind: 'app'
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${mui.id}': {}
    }
  }
  properties:{
    serverFarmId: appServicePlan.id
    keyVaultReferenceIdentity: mui.id
    siteConfig: {
      healthCheckPath: '/health'
      netFrameworkVersion: 'v6.0'
      alwaysOn: true
      keyVaultReferenceIdentity: mui.id
      appSettings:productionSlotAppSettingsFull
      http20Enabled: true
      minTlsVersion: '1.2'
      linuxFxVersion: 'DOTNETCORE|6.0'
      appCommandLine: appCommandLine
    }
    httpsOnly: true
  }
}


// Liaison du DNS Privé
resource privateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' = {
  name: privateDnsZoneName
  location: 'global'
}

resource privateDnsZoneVirtualNetworkLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = {
  name: '${vnet.name}-dnsLink'
  location: 'global'
  parent: privateDnsZone
  properties: {
    virtualNetwork: {
      id: vnet.id
    }
    registrationEnabled: false
  }
}

resource privateEndpointNic 'Microsoft.Network/networkInterfaces@2023-02-01' = {
  name: 'privateAppServiceEndpointNic'
  location: location
  properties: {
    ipConfigurations: [
      {
        name: 'ipconfig1'
        properties: {
          subnet: {
            id: vnet.properties.subnets[3].id // Utilisation du sous-réseau existant
          }
          privateIPAllocationMethod: 'Dynamic' // IP dynamique ou statique
        }
      }
    ]
  }
}

resource privateEndpoint 'Microsoft.Network/privateEndpoints@2023-02-01' = {
  name: privateEndpointName
  location: location
  properties: {
    subnet: {
      id: vnet.properties.subnets[3].id
    }
    privateLinkServiceConnections: [
      {
        name: 'appServicePrivateLink'
        properties: {
          privateLinkServiceId: appService.id
          groupIds: [
            'sites'
          ]
        }
      }
    ]
  }
}

// Créer un enregistrement A dans la zone DNS privée
resource privateDnsZoneRecordSet 'Microsoft.Network/privateDnsZones/A@2024-06-01' = {
  name: '${appServiceName}.${privateDnsZoneName}' // Nom de l'enregistrement (FQDN)
  parent: privateDnsZone
  properties: {
    ttl: 300
    aRecords: [
      {
        ipv4Address: privateEndpointNic.properties.ipConfigurations[0].properties.privateIPAddress
      }
    ]
  }
}
