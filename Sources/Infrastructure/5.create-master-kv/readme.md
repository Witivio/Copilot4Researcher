# Prérequis
Azure CLI doit être [installé](https://learn.microsoft.com/fr-fr/cli/azure/install-azure-cli)
- 
# Creation du service azureOpen AI

Lancer le script `./create-azure-master-kv.ps1` dans une invite de commande powershell
Le script prends 9 parametres obligatoires:
- **Location**: Location ou le service sera créé.
- **SubscriptionId**: Subscription Id de la resource
- **AzureClientId**,
- **AzureClientSecret**,
- **AzureOpenAIEndpoint**,
- **AzureOpenAIApiKey**,
- **AdminDBUsername**,
- **AdminDBPassword**,
- **AzureConnexionServiceAppId**