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
  name: invoiceServiceInfrastructure.databaseName
  params: {
    name: invoiceServiceInfrastructure.databaseName
    location: location
  }
}
