param name string
param location string = resourceGroup().location

module database 'database.bicep' = {
  name: 'database'
  params: {
    name: name
    location: location
  }
}
