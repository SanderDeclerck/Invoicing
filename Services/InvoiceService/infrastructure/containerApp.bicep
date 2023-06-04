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

resource containerAppUserIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
  name: 'id-${containerAppName}'
  location: location
}

resource containerApp 'Microsoft.App/containerApps@2022-11-01-preview' = {
  name: containerAppName
  location: location
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${containerAppUserIdentity.id}': {}
    }
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
          identity: containerAppUserIdentity.id
        }
      ]
      secrets: [
        {
          name: 'invoiceservicecosmosdbconnectionstring'
          identity: containerAppUserIdentity.id
          keyVaultUrl: '${keyVault.properties.vaultUri}secrets/InvoiceServiceCosmosDbConnectionString'
        }
        {
          name: 'honeycombapikey'
          identity: containerAppUserIdentity.id
          keyVaultUrl: '${keyVault.properties.vaultUri}secrets/HoneycombApiKey'
        }
        {
          name: 'honeycomburi'
          identity: containerAppUserIdentity.id
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
              secretRef: 'invoiceservicecosmosdbconnectionstring'
            }            
            {
              name: 'TELEMETRY__HONEYCOMB__APIKEY'
              secretRef: 'honeycombapikey'
            }           
            {
              name: 'TELEMETRY__HONEYCOMB__URI'
              secretRef: 'honeycomburi'
            }
          ]
          probes: [
            {
              httpGet: {
                path: '/healthz'
                port: 80
              }
              initialDelaySeconds: 5
              periodSeconds: 120
              timeoutSeconds: 5
              successThreshold: 1
              failureThreshold: 3
              type: 'Liveness'
            }
            {
              httpGet: {
                path: '/healthz'
                port: 80
              }
              initialDelaySeconds: 5
              periodSeconds: 30
              timeoutSeconds: 5
              successThreshold: 1
              failureThreshold: 3
              type: 'Readiness'
            }
            {
              httpGet: {
                path: '/healthz'
                port: 80
              }
              initialDelaySeconds: 5
              periodSeconds: 30
              timeoutSeconds: 5
              successThreshold: 1
              failureThreshold: 3
              type: 'Startup'
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
  dependsOn: [
    acrRoleAssignment
    keyVaultRbac
  ]
}

module acrRoleAssignment '../../../core-infrastructure/roleAssignments/acrPull.bicep' = {
  name: 'acrPull'
  scope: resourceGroup(coreInfrastructure.resourceGroup)
  params: {
    principalId: containerAppUserIdentity.properties.principalId
    principalType: 'ServicePrincipal'
  }
}

module keyVaultRbac '../../../core-infrastructure/roleAssignments/keyvaultSecretsUser.bicep' = {
  name: 'InvoiceServiceCosmosDbConnectionString'
  scope: resourceGroup(coreInfrastructure.resourceGroup)
  params: {
    principalId: containerAppUserIdentity.properties.principalId
    principalType: 'ServicePrincipal'
  }
}
