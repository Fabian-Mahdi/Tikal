resource "azurerm_resource_group" "current" {
  name     = "${var.global_prefix}-resources"
  location = var.resource_group_location
}

resource "azurerm_virtual_network" "root" {
  name                = "${var.global_prefix}-virtual-network"
  resource_group_name = azurerm_resource_group.current.name
  location            = azurerm_resource_group.current.location
  address_space       = ["10.123.0.0/16"]
}