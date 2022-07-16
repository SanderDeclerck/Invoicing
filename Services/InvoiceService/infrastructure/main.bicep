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
        isZoneRedundant: true
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
