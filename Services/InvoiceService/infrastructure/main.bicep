param location string = resourceGroup().location
param coreInfrastructure object = {
  resourceGroup: ''
  logAnalyticsName: ''
  containerAppEnvName: ''
  containerRegistryName: ''
  keyVaultName: ''
}
param invoiceServiceInfrastructure object = {
  resourceGroup: ''
  databaseName: ''
  containerAppName: ''
}
param imageTag string

module database 'database.bicep' = {
  name: 'database'
  params: {
    name: invoiceServiceInfrastructure.databaseName
    location: location
    keyVaultName: coreInfrastructure.keyVaultName
    keyVaultResourceGroup: coreInfrastructure.resourceGroup
  }
}

module containerApp 'containerApp.bicep' = {
  name: 'containerApp'
  params: {
    location: location
    coreInfrastructure: coreInfrastructure
    containerAppName: invoiceServiceInfrastructure.containerAppName
    imageTag: imageTag
  }
}
