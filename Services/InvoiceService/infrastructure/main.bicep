param location string = resourceGroup().location
param coreInfrastructure object = {
  resourceGroup: ''
  logAnalyticsName: ''
  containerAppEnvName: ''
  containerRegistryName: ''
}
param invoiceServiceInfrastructure object = {
  resourceGroup: ''
  databaseName: ''
  containerAppName: ''
}

module database 'database.bicep' = {
  name: 'database'
  params: {
    name: invoiceServiceInfrastructure.databaseName
    location: location
  }
}

module containerApp 'containerApp.bicep' = {
  name: 'containerApp'
  params: {
    location: location
    coreInfrastructure: coreInfrastructure
    containerAppName: invoiceServiceInfrastructure.containerAppName
  }
}
