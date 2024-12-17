# Paramètres d'entrée
param(
    [Parameter(Mandatory=$true, HelpMessage="Location où la resource sera créée")]
    [string]$Location,
    [Parameter(Mandatory=$true, HelpMessage="Location où la resource sera créée")]
    [string]$SubscriptionId,
    [Parameter(Mandatory=$true, HelpMessage="Location où la resource sera créée")]
    [string]$AzureClientId,
    [Parameter(Mandatory=$true, HelpMessage="Location où la resource sera créée")]
    [string]$AzureClientSecret,
    [Parameter(Mandatory=$true, HelpMessage="Location où la resource sera créée")]
    [string]$AzureOpenAIEndpoint,
    [Parameter(Mandatory=$true, HelpMessage="Location où la resource sera créée")]
    [string]$AzureOpenAIApiKey,
    [Parameter(Mandatory=$true, HelpMessage="Location où la resource sera créée")]
    [string]$AdminDBUsername,
    [Parameter(Mandatory=$true, HelpMessage="Location où la resource sera créée")]
    [string]$AdminDBPassword,
    [Parameter(Mandatory=$true, HelpMessage="Location où la resource sera créée")]
    [string]$AzureConnexionServiceAppId
)

# Variables - personnalisez selon vos besoins
$ResourceGroupName = "cfr-prd"

# Se connecter à Azure (si ce n'est pas encore fait)
Connect-AzAccount
$user = Get-AzContext
$userUpn = $user.Account.Id
Write-Host "userUPN: $userUpn"
$secretValue = "{`"SerpApiApiKey`": { `"value`": `"[ID]`"},`"PubmedApiKeys2`": { `"value`": `"[ID]`"},`"PubmedApiKeys1`": { `"value`": `"[ID]`"}, `"PubmedApiKeys0`": { `"value`": `"[ID]`"}, `"AzureTenantId`": { `"value`": `"[ID]`"}, `"AzureClientId`": { `"value`": `"$AzureClientId`"}, `"AzureClientSecret`": { `"value`": `"$AzureClientSecret`"}, `"SharePointUrl`": { `"value`": `"https://witiviotap.sharepoint.com/c4r/`"}, `"SharePointListId`": { `"value`": `"[ID]`"}, `"SharePointDeliveryNotesUrl`": { `"value`": `"https://witiviotap.sharepoint.com/c4r/Shared%20Documents`"}, `"SharePointLogListId`": { `"value`": `"[ID]`"},`"SharePointScanIntervalMinutes`": { `"value`": `"1`"},`"AzureOpenAIChatDeploymentName`": { `"value`": `"gpt4-o`"},`"AzureOpenAIEndpoint`": { `"value`": `"$AzureOpenAIEndpoint`"},`"AzureOpenAIApiKey`": { `"value`": `"$AzureOpenAIApiKey`"},`"ConnectionStrings`": { `"value`": `"Server=cfr-postgres-prd.postgres.database.azure.com;Database=postgres;Port=5432;User Id=$AdminDBUsername;Password=$AdminDBPassword;Ssl Mode=Require;`"},`"deploymentEnvironment`": { `"value`": `"prd`" }, `"postgresAdminUsername`": { `"value`": `"$AdminDBUsername`" },     `"postgresAdminPassword`": { `"value`": `"$AdminDBPassword`" }}"
Select-AzSubscription -SubscriptionId $SubscriptionId

# Créer le groupe de ressources
Write-Host "Création du service Keyvault dans la région '$Location'..."
New-AzKeyVault -Name cfr-master-kv -ResourceGroupName $ResourceGroupName -Location $Location -EnableRbacAuthorization
New-AzRoleAssignment -SignInName $userUpn -RoleDefinitionName "Key Vault Secrets Officer" -Scope $scope
#Write-Host "Keyvault créé avec succès."
Set-AzKeyVaultSecret -VaultName cfr-master-kv -Name cfr-bicepparam-prd -SecretValue (ConvertTo-SecureString $secretValue -AsPlainText -Force) -ContentType application/json
# Récupérez l'ID du Key Vault
$scope = "/subscriptions/$SubscriptionId/resourceGroups/$ResourceGroupName/providers/Microsoft.KeyVault/vaults/cfr-master-kv"

New-AzRoleAssignment -ApplicationId $AzureConnexionServiceAppId -RoleDefinitionName "Key Vault Secrets Officer" -Scope $scope