param location string = resourceGroup().location
param containerAppName string
param coreInfrastructure object = {
  resourceGroup: ''
  logAnalyticsName: ''
  containerAppEnvName: ''
  containerRegistryName: ''
}

resource containerAppEnv 'Microsoft.App/managedEnvironments@2022-11-01-preview' existing = {
  name: coreInfrastructure.containerAppEnvName
  scope: resourceGroup(coreInfrastructure.resourceGroup)
}

resource containerApp 'Microsoft.App/containerApps@2022-11-01-preview' = {
  name: containerAppName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    managedEnvironmentId: containerAppEnv.id
  }
}

module acrRoleAssignment '../../../core-infrastructure/roleAssignments/acrPull.bicep' = {
  name: 'acrPull'
  scope: resourceGroup(coreInfrastructure.resourceGroup)
  params: {
    principalId: containerApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

module keyVaultRbac '../../../core-infrastructure/roleAssignments/keyvaultSecretsUser.bicep' = {
  name: 'InvoiceServiceCosmosDbConnectionString'
  scope: resourceGroup(coreInfrastructure.resourceGroup)
  params: {
    principalId: containerApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}
