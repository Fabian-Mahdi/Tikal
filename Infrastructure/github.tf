locals {
  github_prefix = "github"
}

resource "azurerm_subnet" "github" {
  name                 = "${var.global_prefix}-${local.github_prefix}-subnet"
  resource_group_name  = azurerm_resource_group.current.name
  virtual_network_name = azurerm_virtual_network.root.name

  address_prefixes = ["10.123.5.0/24"]

  delegation {
    name = "delegation"

    service_delegation {
      name    = "GitHub.Network/networkSettings"
      actions = ["Microsoft.Network/virtualNetworks/subnets/join/action"]
    }
  }
}

resource "azapi_resource" "github_network_settings" {
  type                      = "GitHub.Network/networkSettings@2024-04-02"
  name                      = "${var.global_prefix}-${local.github_prefix}-network-settings"
  location                  = azurerm_resource_group.current.location
  parent_id                 = azurerm_resource_group.current.id
  schema_validation_enabled = false
  body = jsonencode({
    properties = {
      businessId = var.github_database_id
      subnetId   = azurerm_subnet.github.id
    }
  })
  response_export_values = ["tags.GitHubId"]

  lifecycle {
    ignore_changes = [tags]
  }
}

resource "azuread_application" "github" {
  display_name = "${var.global_prefix}-${local.github_prefix}-application"
  owners       = [data.azuread_client_config.current.object_id]
}

resource "azuread_service_principal" "github" {
  client_id                    = azuread_application.github.client_id
  app_role_assignment_required = false
  owners                       = [data.azuread_client_config.current.object_id]
}

resource "azuread_application_password" "github" {
  application_id = azuread_application.github.id
}

resource "azurerm_key_vault_access_policy" "github-identity" {
  key_vault_id = azurerm_key_vault.current.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = azuread_service_principal.github.object_id

  secret_permissions = [
    "Get",
    "List",
  ]
}

resource "azurerm_key_vault_access_policy" "github-tikal-backend" {
  key_vault_id = azurerm_key_vault.tikal.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = azuread_service_principal.github.object_id

  secret_permissions = [
    "Get",
    "List",
  ]
}

resource "azurerm_role_assignment" "github" {
  scope                = data.azurerm_subscription.current.id
  role_definition_name = "Contributor"
  principal_id         = azuread_service_principal.github.object_id
}
