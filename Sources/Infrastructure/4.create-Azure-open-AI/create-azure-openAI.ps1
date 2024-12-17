# Paramètres d'entrée
param(
    [Parameter(Mandatory=$true, HelpMessage="Location où la resource sera créée")]
    [string]$Location,
    [Parameter(Mandatory=$true, HelpMessage="Location où la resource sera créée")]
    [string]$SubscriptionId
)

# Variables - personnalisez selon vos besoins
$ResourceGroupName = "cfr-prd"

# Se connecter à Azure (si ce n'est pas encore fait)
Connect-AzAccount

Select-AzSubscription -SubscriptionId $SubscriptionId

# Créer le groupe de ressources
Write-Host "Création du service azure OpenAI dans la région '$Location'..."
New-AzCognitiveServicesAccount -ResourceGroupName $ResourceGroupName -Name cfr-copilotforresearcher-prd -Type OpenAI -SkuName S0 -Location $Location
Write-Host "Azure open AI service créé avec succès."
$endpointName = Get-AzCognitiveServicesAccount -ResourceGroupName $ResourceGroupName -Name cfr-copilotforresearcher-prd | Select-Object -Property endpoint
$apiKey = Get-AzCognitiveServicesAccountKey -Name cfr-copilotforresearcher-prd -ResourceGroupName $ResourceGroupName | Select-Object -Property Key1
Write-Host "    EndpointName: $endpointName"
Write-Host "    ApiKey: $apiKey"

