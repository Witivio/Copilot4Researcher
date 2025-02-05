
# Paramètres d'entrée
param(
    [Parameter(Mandatory=$true, HelpMessage="Location où la resource sera créée")]
    [string]$Location,
    [Parameter(Mandatory=$true, HelpMessage="SubscriptionId où la resource sera créée")]
    [string]$SubscriptionId
)

# Variables - personnalisez selon vos besoins
$ResourceGroupName = "cfr-prd"

# Se connecter à Azure (si ce n'est pas encore fait)
Connect-AzAccount

# Sélectionner la souscription spécifique
Select-AzSubscription -SubscriptionId $SubscriptionId

# Vérifier si le groupe de ressources existe déjà
$resourceGroup = Get-AzResourceGroup -Name $ResourceGroupName -ErrorAction SilentlyContinue

if ($null -eq $resourceGroup) {
    # Créer le groupe de ressources
    Write-Host "Création du groupe de ressources '$ResourceGroupName' dans la région '$Location' sous la souscription '$SubscriptionId'..."
    New-AzResourceGroup -Name $ResourceGroupName -Location $Location
    Write-Host "Groupe de ressources créé avec succès."
} else {
    Write-Host "Le groupe de ressources '$ResourceGroupName' existe déjà."
}
