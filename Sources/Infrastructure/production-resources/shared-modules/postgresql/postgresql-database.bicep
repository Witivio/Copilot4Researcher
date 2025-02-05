@description('PostgreSQL server name')
param serverName string

@description('PostgreSQL database name')
param databaseName string

@description('PostgreSQL database user name')
param adminUsername string

@description('PostgreSQL database user password')
@secure()
param adminPassword string

// @description('PostgreSQL database user name')
// param databaseUserName string

// @description('PostgreSQL database user password')
// @secure()
// param databaseUserPassword string

// param location string

resource postgresqlServer 'Microsoft.DBforPostgreSQL/flexibleServers@2022-12-01' existing = {
  name: serverName
  
}

resource postgresqlDatabase 'Microsoft.DBforPostgreSQL/flexibleServers/databases@2022-12-01' = {
  name: databaseName
  parent: postgresqlServer
  properties:{
    charset: 'UTF8'
    collation: 'en_US.utf8'
  }
}

// resource postgreslMui 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
//   name: '${appPrefix}-psql-${databaseName}-mui-${deploymentEnvironment}'
//   location: location
// }

// resource assignIdentity 'Microsoft.DBforPostgreSQL/flexibleServers/administrators@2022-12-01' = {
//   parent: postgresqlServer
//   name: 'activeDirectory'
//   properties: {
//     principalName: postgreslMui.properties.principalId
//     principalType: 'UserAssigned'
//   }
// }

var serverHost = 'Server=${postgresqlServer.properties.fullyQualifiedDomainName};'
var database = 'Database=${postgresqlDatabase.name};'
var user = 'User ID=${adminUsername};'
var password = 'Password=${adminPassword};'
var extra = 'Integrated Security=true;Pooling=true;'

var connectionString = '${serverHost}${database}${user}${password}${extra}'

// var connectionString = 'User ID=${databaseUserName};Password=${databaseUserPassword};Server=${postgresqlServer.properties.fullyQualifiedDomainName};Port=5432;Database=${postgresqlDatabase.name};Integrated Security=true;Pooling=true;'
// output connectionString string = secure('Server=' + postgresqlServer.properties.fullyQualifiedDomainName + ';Database=' + databaseName + ';User ID=' + managedIdentity.name + '@' + flexibleServer.properties.tenantId + ';Authentication=Active Directory Password;')

output connectionString string = connectionString

