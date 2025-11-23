resource "azurerm_private_dns_zone" "dns_private" {
  name                = "privatelink.azurewebsites.net"
  resource_group_name = azurerm_resource_group.current.name
}

resource "azurerm_private_dns_zone_virtual_network_link" "dns_private" {
  name                  = "${var.global_prefix}-private-dnszonelink"
  resource_group_name   = azurerm_resource_group.current.name
  private_dns_zone_name = azurerm_private_dns_zone.dns_private.name
  virtual_network_id    = azurerm_virtual_network.root.id
}

resource "azurerm_dns_zone" "dns_public" {
  name                = var.domain_name
  resource_group_name = azurerm_resource_group.current.name
}

resource "azurerm_dns_a_record" "application_gateway_root" {
  name                = "@"
  zone_name           = azurerm_dns_zone.dns_public.name
  resource_group_name = azurerm_resource_group.current.name
  ttl                 = 300
  records             = [azurerm_public_ip.application-gateway.ip_address]
}

resource "azurerm_dns_a_record" "application_gateway_auth" {
  name                = "auth"
  zone_name           = azurerm_dns_zone.dns_public.name
  resource_group_name = azurerm_resource_group.current.name
  ttl                 = 300
  records             = [azurerm_public_ip.application-gateway.ip_address]
}

resource "azurerm_dns_a_record" "application_gateway_backend" {
  name                = "backend"
  zone_name           = azurerm_dns_zone.dns_public.name
  resource_group_name = azurerm_resource_group.current.name
  ttl                 = 300
  records             = [azurerm_public_ip.application-gateway.ip_address]
}

resource "azurerm_dns_a_record" "application_gateway_otel" {
  name                = "otel"
  zone_name           = azurerm_dns_zone.dns_public.name
  resource_group_name = azurerm_resource_group.current.name
  ttl                 = 300
  records             = [azurerm_public_ip.application-gateway.ip_address]
}
