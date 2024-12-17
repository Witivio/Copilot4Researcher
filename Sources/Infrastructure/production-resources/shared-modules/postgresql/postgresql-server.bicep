
param serverName string
param adminUsername string
@secure()
param adminPassword string
param location string
param muiName string
param vnetName string
param subnetName string


resource mui 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' existing = {
  name: muiName
}


resource vnet 'Microsoft.Network/virtualNetworks@2023-05-01' existing = {
  name: vnetName
}

resource postgresqlServer 'Microsoft.DBforPostgreSQL/flexibleServers@2022-12-01' = {
  location:location
  name: serverName
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities:{
      '${mui.id}':{}
    }
  }
  sku: {
    name: 'Standard_B1ms'
    tier: 'Burstable'
  }
  properties:{
    version: '15'
    administratorLogin: adminUsername
    administratorLoginPassword: adminPassword
    highAvailability:{
      mode: 'Disabled'
    }
    storage:{
      storageSizeGB: 32
    }
    backup:{
      backupRetentionDays: 7
      geoRedundantBackup: 'Disabled'
    }
    availabilityZone: '1'
    network: {
      delegatedSubnetResourceId: resourceId('Microsoft.Network/virtualNetworks/subnets', vnetName, subnetName)
      privateDnsZoneArmResourceId: resourceId('Microsoft.Network/privateDnsZones', 'privatelink.postgres.database.azure.com')
    }
  }
}

resource privateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' = {
  name: 'privatelink.postgres.database.azure.com'
  location: 'global'
  properties: {}
}

resource virtualNetworkLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = {
  name: 'myVNetLink'
  location: 'global'
  parent: privateDnsZone
  properties: {
    virtualNetwork: {
      id: vnet.id
    }
    registrationEnabled: false
  }
}

resource postgresqlfirewallRule 'Microsoft.DBforPostgreSQL/flexibleServers/firewallRules@2023-03-01-preview' = {
  name: 'AllowAllWindowsAzureIps'
  parent: postgresqlServer
  properties: {
    endIpAddress: '0.0.0.0'
    startIpAddress: '0.0.0.0'
  }
}



output fullyQualifiedDomainName string = postgresqlServer.properties.fullyQualifiedDomainName
output serverName string = postgresqlServer.name
