
param location string = resourceGroup().location
@description('The name of the log analytics workspace to use for the app.')
param logWorkspaceName string
@description('The name of the service to use for the app insight.')
param serviceName string
@description('The URL to use for the health check. it must start with https:// and hit your health check endpoint.')
param appServiceHealthCheckUrl string

param deploymentEnvironment string

var availabilityTestName = '${serviceName}-availability-test'


resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2020-08-01' existing = {
  name: logWorkspaceName
}


resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: serviceName
  location: location
  kind: 'web'
  properties:{
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspace.id
    RetentionInDays:30
    IngestionMode: 'LogAnalytics'
  }
}

resource appInsightAvailabilityTests 'Microsoft.Insights/webtests@2022-06-15' = if (deploymentEnvironment == 'prd') {
  name: availabilityTestName
  location: location
  kind: 'standard'
  tags: {
    'hidden-link:${appInsights.id}': 'Resource'
  }
  properties:{
    Configuration: {
      WebTest:''
    }
    Kind:'standard'
    Name: 'Health Check'
    SyntheticMonitorId: availabilityTestName
    Enabled:true
    Frequency: 300
    Locations: [
      {
        Id: 'emea-nl-ams-azr'
      }
      {
        Id: 'emea-gb-db3-azr'
      }
    ]
    Request:{
      HttpVerb:'GET'
      RequestUrl: appServiceHealthCheckUrl
    }
    RetryEnabled: true
    ValidationRules:{
      ContentValidation:{
        ContentMatch:'Healthy'
        IgnoreCase:true
      }
      ExpectedHttpStatusCode:200
      SSLCheck:true
    }
  }
}


output appInsightProperties object = appInsights.properties
