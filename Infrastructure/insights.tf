locals {
  insights_prefix = "insights"
}

resource "azurerm_application_insights" "current" {
  name                = "${var.global_prefix}-${local.insights_prefix}"
  location            = azurerm_resource_group.current.location
  resource_group_name = azurerm_resource_group.current.name
  application_type    = "web"
}

resource "azurerm_key_vault_secret" "insights-connection-string" {
  key_vault_id = azurerm_key_vault.current.id
  name         = "AzureInsightsConnectionString"
  content_type = "text/plain"
  value        = azurerm_application_insights.current.connection_string
}