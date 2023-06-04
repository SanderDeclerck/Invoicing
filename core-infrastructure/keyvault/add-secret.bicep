param keyVaultName string
param key string
@secure()
param value string

resource keyVault 'Microsoft.KeyVault/vaults@2023-02-01' existing = {
  name: keyVaultName
}

resource secret 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  name: key
  parent: keyVault
  properties: {
    value: value
  }
}
