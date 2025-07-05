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

resource "azurerm_user_assigned_identity" "github" {
  resource_group_name = azurerm_resource_group.current.name
  location            = azurerm_resource_group.current.location
  name                = "${var.global_prefix}-${local.github_prefix}-identity"
}