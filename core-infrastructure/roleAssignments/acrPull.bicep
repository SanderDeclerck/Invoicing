param principalId string
@allowed(['ServicePrincipal', 'User', 'Group'])
param principalType string

var acrPullRole = resourceId('Microsoft.Authorization/roleDefinitions', '7f951dda-4ed3-4680-a7ca-43fe172d538d')

resource containerRegistryRbac 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(resourceGroup().id, principalId, acrPullRole)
  properties: {
    principalId: principalId
    principalType: principalType
    roleDefinitionId: acrPullRole
  }
}
