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
  storageAccountName: ''
}
param imageTag string

module containerApp 'containerApp.bicep' = {
  name: 'containerApp'
  params: {
    location: location
    coreInfrastructure: coreInfrastructure
    containerAppName: invoiceServiceInfrastructure.containerAppName
    imageTag: imageTag
  }
  dependsOn: [
    database
    storageAccount
  ]
}

module database 'database.bicep' = {
  name: 'database'
  params: {
    name: invoiceServiceInfrastructure.databaseName
    location: location
    keyVaultName: coreInfrastructure.keyVaultName
    keyVaultResourceGroup: coreInfrastructure.resourceGroup
  }
}

module storageAccount 'storageAccount.bicep' = {
  name: 'storageAccount'
  params: {
    storageAccountName: invoiceServiceInfrastructure.storageAccountName
    location: location
    keyVaultName: coreInfrastructure.keyVaultName
    keyVaultResourceGroup: coreInfrastructure.resourceGroup
  }
}
