resource "azurerm_subnet" "services" {
  name                 = "${var.global_prefix}-service-subnet"
  resource_group_name  = azurerm_resource_group.current.name
  virtual_network_name = azurerm_virtual_network.root.name
  address_prefixes     = ["10.123.20.0/24"]

  delegation {
    name = "${var.global_prefix}-service-subnet-delegation"

    service_delegation {
      name    = "Microsoft.Web/serverFarms"
      actions = ["Microsoft.Network/virtualNetworks/subnets/action"]
    }
  }
}
