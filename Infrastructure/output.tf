data "azurerm_client_config" "current" {}

data "azurerm_subscription" "current" {}

data "azuread_client_config" "current" {}

output "github_network_settings_id" {
  description = "ID of the GitHub.Network/networkSettings resource"
  value       = jsondecode(azapi_resource.github_network_settings.output).tags.GitHubId
}