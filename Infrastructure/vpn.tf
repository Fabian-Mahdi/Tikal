locals {
  vpn_prefix = "vpn-gateway"
}

resource "azurerm_subnet" "vpn-gateway" {
  name                 = "GatewaySubnet"
  resource_group_name  = azurerm_resource_group.current.name
  virtual_network_name = azurerm_virtual_network.root.name
  address_prefixes     = ["10.123.100.0/24"]
}

resource "azurerm_public_ip" "vpn-gateway" {
  name                = "${var.global_prefix}-${local.vpn_prefix}-pip"
  location            = azurerm_resource_group.current.location
  resource_group_name = azurerm_resource_group.current.name

  sku               = "Standard"
  allocation_method = "Static"
}

resource "azurerm_virtual_network_gateway" "vpn-gateway" {
  name                = "${var.global_prefix}-${local.vpn_prefix}"
  location            = azurerm_resource_group.current.location
  resource_group_name = azurerm_resource_group.current.name

  type     = "Vpn"
  vpn_type = "RouteBased"

  active_active = false
  enable_bgp    = false
  sku           = "VpnGw1"

  ip_configuration {
    name                          = "${var.global_prefix}-${local.vpn_prefix}-ip-config"
    public_ip_address_id          = azurerm_public_ip.vpn-gateway.id
    private_ip_address_allocation = "Dynamic"
    subnet_id                     = azurerm_subnet.vpn-gateway.id
  }

  vpn_client_configuration {
    address_space = ["10.0.1.0/24"]

    vpn_client_protocols = ["OpenVPN"]

    aad_tenant   = "https://login.microsoftonline.com/${data.azurerm_client_config.current.tenant_id}/"
    aad_issuer   = "https://sts.windows.net/${data.azurerm_client_config.current.tenant_id}/"
    aad_audience = var.vpn_application_id

    vpn_auth_types = ["AAD"]
  }
}