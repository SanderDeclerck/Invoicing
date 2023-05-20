param name string
param location string = resourceGroup().location

resource cosmosDb 'Microsoft.DocumentDB/databaseAccounts@2022-05-15' = {
  name: '${name}-db'
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
