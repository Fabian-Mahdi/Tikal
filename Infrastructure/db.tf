locals {
  db_prefix          = "db"
  identity_db_prefix = "identity-db"
}

resource "azurerm_subnet" "db" {
  name                 = "${var.global_prefix}-${local.db_prefix}-subnet"
  resource_group_name  = azurerm_resource_group.current.name
  virtual_network_name = azurerm_virtual_network.root.name
  address_prefixes     = ["10.123.4.0/24"]

  service_endpoints = ["Microsoft.Storage"]
  delegation {
    name = "fs"
    service_delegation {
      name = "Microsoft.DBforPostgreSQL/flexibleServers"
      actions = [
        "Microsoft.Network/virtualNetworks/subnets/join/action",
      ]
    }
  }
}

resource "azurerm_private_dns_zone" "db" {
  name                = "privatelink.postgres.database.azure.com"
  resource_group_name = azurerm_resource_group.current.name
}

resource "azurerm_private_dns_zone_virtual_network_link" "db" {
  name                  = "${var.global_prefix}-${local.db_prefix}-private-dnszonelink"
  resource_group_name   = azurerm_resource_group.current.name
  private_dns_zone_name = azurerm_private_dns_zone.db.name
  virtual_network_id    = azurerm_virtual_network.root.id
}

resource "azurerm_postgresql_flexible_server" "db" {
  name                          = "${var.global_prefix}-${local.db_prefix}-server"
  resource_group_name           = azurerm_resource_group.current.name
  location                      = azurerm_resource_group.current.location
  version                       = "16"
  delegated_subnet_id           = azurerm_subnet.db.id
  private_dns_zone_id           = azurerm_private_dns_zone.db.id
  administrator_login           = var.db_admin_login
  administrator_password        = random_password.db_admin_password.result
  storage_mb                    = 32768
  sku_name                      = "B_Standard_B1ms"
  backup_retention_days         = 7
  public_network_access_enabled = false
  zone                          = "2"

  depends_on = [azurerm_private_dns_zone_virtual_network_link.db]
}

resource "azurerm_postgresql_flexible_server_database" "identity-db" {
  name      = var.identity_db_name
  server_id = azurerm_postgresql_flexible_server.db.id
  collation = "en_US.utf8"
  charset   = "UTF8"

  lifecycle {
    prevent_destroy = false
  }
}

resource "random_password" "db_admin_password" {
  length           = 20
  special          = true
  lower            = true
  upper            = true
  override_special = "!#"
}

# Secrets

resource "azurerm_key_vault_secret" "db_admin_password" {
  key_vault_id = azurerm_key_vault.current.id
  name         = "dbpassword"
  content_type = "text/plain"
  value        = random_password.db_admin_password.result
}