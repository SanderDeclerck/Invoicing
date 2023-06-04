param location string = resourceGroup().location
param containerAppName string
param coreInfrastructure object = {
  resourceGroup: ''
  logAnalyticsName: ''
  containerAppEnvName: ''
  containerRegistryName: ''
  keyVaultName: ''
}
param imageTag string

resource containerAppEnv 'Microsoft.App/managedEnvironments@2022-11-01-preview' existing = {
  name: coreInfrastructure.containerAppEnvName
  scope: resourceGroup(coreInfrastructure.resourceGroup)
}

resource keyVault 'Microsoft.KeyVault/vaults@2023-02-01' existing = {
  name: coreInfrastructure.keyVaultName
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
    configuration: {
      ingress: {
        external: true
        allowInsecure: false
        targetPort: 80
        traffic: [
          {
            latestRevision: true
            weight: 100
          }
        ]
      }
      registries: [
        {
          server: '${coreInfrastructure.containerRegistryName}.azurecr.io'
          identity: 'system'
        }
      ]
      secrets: [
        {
          name: 'InvoiceServiceCosmosDbConnectionString'
          identity: 'system'
          keyVaultUrl: '${keyVault.properties.vaultUri}secrets/InvoiceServiceCosmosDbConnectionString'
        }
        {
          name: 'HoneycombApiKey'
          identity: 'system'
          keyVaultUrl: '${keyVault.properties.vaultUri}secrets/HoneycombApiKey'
        }
        {
          name: 'HoneycombUri'
          identity: 'system'
          keyVaultUrl: '${keyVault.properties.vaultUri}secrets/HoneycombUri'
        }
      ]
    }
    template: {
      containers: [
        {
          name: containerAppName
          image: '${coreInfrastructure.containerRegistryName}.azurecr.io/invoice-service:${imageTag}'
          resources: {
            cpu: json('0.25')
            memory: '0.5Gi'
          }
          env: [
            {
              name: 'ASPNETCORE_ENVIRONMENT'
              value: 'Production'
            }
            {
              name: 'CONNECTIONSTRINGS__INVOICES'
              secretRef: 'InvoiceServiceCosmosDbConnectionString'
            }            
            {
              name: 'TELEMETRY__HONEYCOMB__APIKEY'
              secretRef: 'HoneycombApiKey'
            }           
            {
              name: 'TELEMETRY__HONEYCOMB__URI'
              secretRef: 'HoneycombUri'
            }
          ]
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 3
      }
    }
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
