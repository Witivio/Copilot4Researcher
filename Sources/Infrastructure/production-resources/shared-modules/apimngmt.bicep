@description('Nom unique du service API Management')
param serviceName string

@description('Région pour déployer le service API Management')
param location string = resourceGroup().location

var apiName = 'cfr-api'

var backendName = 'cfr-backend'

var apiVersion  = 'v1'

var apiPath  = '/v1'

var operationName = 'get-items'

resource appService 'Microsoft.Web/sites@2022-03-01' existing = {
  name: serviceName
}


resource apiManagement 'Microsoft.ApiManagement/service@2023-03-01-preview' = {
  name: '${serviceName}-apimgnt'
  location: location
  sku: {
    name: 'StandardV2'
    capacity: 1
  }
  properties: {
    publisherEmail: 'nhumann@witivio.com'
    publisherName: 'NHN'
  }
}

resource api 'Microsoft.ApiManagement/service/apis@2021-12-01-preview' = {
  parent: apiManagement
  name: apiName
  properties: {
    displayName: 'Copilot for Reseacher API'
    serviceUrl: 'https://${appService.name}.azurewebsites.net'
    path: apiPath
    protocols: [
      'https'
    ]
  }
}

resource backend 'Microsoft.ApiManagement/service/backends@2021-12-01-preview' = {
  parent: apiManagement
  name: backendName
  properties: {
    url: 'https://${appService.name}.azurewebsites.net'
    protocol: 'http'
  }
}

resource apiOperation 'Microsoft.ApiManagement/service/apis/operations@2021-12-01-preview' = {
  parent: api
  name: operationName
  properties: {
    displayName: 'Search clinical trial'
    method: 'GET'
    urlTemplate: '/search'
    request: {
      queryParameters: [
        {
          name: 'keywords'
          type: 'array'
          required: false
        }
        {
          name: 'userIntent'
          type: 'string'
          required: false
        }
        {
          name: 'userInput'
          type: 'string'
          required: false
        }
        {
          name: 'nbItems'
          type: 'integer'
          required: false
        }
        {
          name: 'conditions'
          type: 'string'
          required: false
        }
        {
          name: 'intervention'
          type: 'string'
          required: false
        }
        {
          name: 'outcomeMeasure'
          type: 'string'
          required: false
        }
        {
          name: 'sponsor'
          type: 'string'
          required: false
        }
        {
          name: 'leadSponsorName'
          type: 'string'
          required: false
        }
        {
          name: 'id'
          type: 'string'
          required: false
        }
      ]
    }
    responses: [
      {
        statusCode: 200
        description: 'OK'
      }
    ]
  }
}


resource apiPolicy 'Microsoft.ApiManagement/service/apis/policies@2021-12-01-preview' = {
  parent: api
  name: 'policy'
  properties: {
    format: 'xml'
    value: '''
<policies>
  <inbound>
    <base />
    <set-backend-service base-url='https://cfr-copilotforresearcher-prd.azurewebsites.net' />
  </inbound>
  <backend>
    <base />
  </backend>
  <outbound>
    <base />
  </outbound>
</policies>
'''
  }
}

output apiManagementResourceId string = apiManagement.id
