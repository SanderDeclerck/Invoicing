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
    }
    template: {
      containers: [
        {
          name: containerAppName
          image: 'mcr.microsoft.com/azuredocs/containerapps-helloworld:latest'
          resources: {
            cpu: json('0.25')
            memory: '0.5Gi'
          }
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
