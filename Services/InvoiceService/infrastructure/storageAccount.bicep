param storageAccountName string
param location string = resourceGroup().location
param keyVaultName string
param keyVaultResourceGroup string

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-04-01' = {
  name: storageAccountName
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_GRS'
  }
}

resource blobService 'Microsoft.Storage/storageAccounts/blobServices@2021-04-01' = {
  name: 'default'
  parent: storageAccount
}

resource logosContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-04-01' = {
  name: 'invoice-issuers-logos'
  parent: blobService
}

module keyVaultSecret '../../../core-infrastructure/keyvault/add-secret.bicep' = {
  name: 'InvoiceServiceCosmosDbConnectionString'
  scope: resourceGroup(keyVaultResourceGroup)
  params: {
    key: 'InvoiceServiceStorageAccountConnectionString'
    keyVaultName: keyVaultName
    value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${storageAccount.listKeys().keys[0].value};EndpointSuffix=core.windows.net'
  }
}
