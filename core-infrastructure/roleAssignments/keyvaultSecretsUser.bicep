param principalId string
@allowed(['ServicePrincipal', 'User', 'Group'])
param principalType string

var keyVaultSecretsUserRole = resourceId('Microsoft.Authorization/roleDefinitions', '4633458b-17de-408a-b874-0445c86b69e6')

resource keyVaultSecretsUserRbac 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(resourceGroup().id, principalId, keyVaultSecretsUserRole)
  properties: {
    principalId: principalId
    principalType: principalType
    roleDefinitionId: keyVaultSecretsUserRole
  }
}
