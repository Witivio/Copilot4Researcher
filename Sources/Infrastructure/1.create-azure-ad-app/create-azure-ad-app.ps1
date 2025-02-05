
# Paramètres d'entrée
param(
    [Parameter(Mandatory=$true, HelpMessage="L'ID du tenant Azure AD où l'application sera créée")]
    [string]$tenantId
)

# connexion à Azure
Connect-AzAccount

$appname = "cfr-copilotforresearcher-prd"
$dns = "cfr-copilotforresearcher-prd-apimgt.azure-api.net"
$baseUrl = "https://cfr-copilotforresearcher-prd-apimgt.azure-api.net"
Write-Host "Création de l'application $appname dans le tenant Azure AD $tenantId à partir du fichier apipermission.json"
$appId = az ad app create --display-name "${appname}" --sign-in-audience 'AzureADMultipleOrgs' --required-resource-accesses apiPermissions.json --enable-access-token-issuance false --query "appId" --output tsv
Write-Host "L'application $appId a été créée"
$appOId = az ad app show --id $APPID --query id --output tsv
Write-Host "L'application $appId a l'ID d'objet $appOId"

Write-Host "Création du secret de l'application"
$appSecret= az ad app credential reset --id $APPID --append --display-name production --years 1 --query password --output tsv

Write-Host "Préparation du manifeste"
Copy-Item "adManifest.json" "preparedManifest.json"
(Get-Content "preparedManifest.json") -replace "<<AppId>>", "$appId" -replace "<<Domain>>", "$dns" | Set-Content "preparedManifest.json"
Get-Content "preparedManifest.json"

# Add scopes.
Write-Host "Ajout des redirect uris et de l'identifiant d'api à partir du fichier adManifest.json"
az rest --method PATCH --uri "https://graph.microsoft.com/v1.0/applications/$appOId" --headers "Content-Type=application/json" --body `@preparedManifest.json

Write-Host "Ajout des applicationIds de Teams a partir du fichier preAuthorizedApplications.json"
# Add preauthorized client. Here Teams web client and Teams desktop Client are preauthorized.
az rest --method PATCH --uri "https://graph.microsoft.com/v1.0/applications/$appOId" --headers "Content-Type=application/json" --body `@preAuthorizedApplications.json

Remove-Item preparedManifest.json

# Display the results
Write-Host "L'application Azure AD multi-tenant a été créée avec succès :"
Write-Host "    ClientId: $appId"
Write-Host "    Secret: $appSecret"
Write-host "    Internal Api Scope: api://$dns/$appId/.default"
Write-Host "    URL pour consentir l'application: https://login.microsoftonline.com/common/adminconsent?client_id=$appId&redirect_uri=$baseUrl"