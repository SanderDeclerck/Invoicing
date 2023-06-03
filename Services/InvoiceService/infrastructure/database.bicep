param location string = resourceGroup().location
param name string
param keyVaultName string
param keyVaultResourceGroup string

resource cosmosDb 'Microsoft.DocumentDB/databaseAccounts@2022-05-15' = {
  name: name
  location: location
  kind: 'GlobalDocumentDB'
  properties: {
    databaseAccountOfferType: 'Standard'
    locations: [
      {
        isZoneRedundant: false
        locationName: location
      }
    ]
    backupPolicy: {
      type: 'Continuous'
    }
    capabilities: [
      {
        name: 'EnableServerless'
      }
    ]
  }
}

resource invoiceDatabase 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2023-04-15' = {
  name: 'InvoiceService'
  parent: cosmosDb
  location: location
  properties: {
    resource: {
      id: 'InvoiceService'
    }
  }
}

resource invoicesDatabaseContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2023-04-15' = {
  name: 'Invoices'
  parent: invoiceDatabase
  location: location
  properties: {
    resource: {
      id: 'Invoices'
      partitionKey: {
        paths: [
          '/tenantId'
        ]
        kind: 'Hash'
      }
    }
  }
}

resource invoiceNumberSourcesContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2023-04-15' = {
  name: 'InvoiceNumberSources'
  parent: invoiceDatabase
  location: location
  properties: {
    resource: {
      id: 'InvoiceNumberSources'
      partitionKey: {
        paths: [
          '/tenantId'
        ]
        kind: 'Hash'
      }
    }
  }
}


module keyVaultSecret '../../../core-infrastructure/keyvault/add-secret.bicep' = {
  name: 'InvoiceServiceCosmosDbConnectionString'
  scope: resourceGroup(keyVaultResourceGroup)
  params: {
    key: 'InvoiceServiceCosmosDbConnectionString'
    keyVaultName: keyVaultName
    value: 'AccountEndpoint=${cosmosDb.properties.documentEndpoint};AccountKey=${cosmosDb.listKeys().primaryMasterKey}'
  }
}
