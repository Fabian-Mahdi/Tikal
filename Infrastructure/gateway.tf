locals {
  gateway_prefix = "application-gateway"

  gateway_ip_configuration_name  = "${var.global_prefix}-${local.gateway_prefix}-ip-configuration"
  frontend_port_name             = "${var.global_prefix}-${local.gateway_prefix}-frontend.port"
  frontend_ip_configuration_name = "${var.global_prefix}-${local.gateway_prefix}-frontend-ip-configuration"
  backend_address_pool_name      = "${var.global_prefix}-${local.gateway_prefix}-backend-address-pool"
  probe_name                     = "${var.global_prefix}-${local.gateway_prefix}-probe"
  backend_http_settings_name     = "${var.global_prefix}-${local.gateway_prefix}-backend-http-settings"
  http_listener_name             = "${var.global_prefix}-${local.gateway_prefix}-http-listener"
  routing_rule_name              = "${var.global_prefix}-${local.gateway_prefix}-routing_rule"
  certificate_name               = "${var.global_prefix}-${local.gateway_prefix}-certificate"
}

resource "azurerm_subnet" "application-gateway" {
  name                 = "${var.global_prefix}-${local.gateway_prefix}-subnet"
  resource_group_name  = azurerm_resource_group.current.name
  virtual_network_name = azurerm_virtual_network.root.name
  address_prefixes     = ["10.123.1.0/24"]

  service_endpoints = ["Microsoft.Web"]
}

resource "azurerm_public_ip" "application-gateway" {
  name                = "${var.global_prefix}-${local.gateway_prefix}-public-ip"
  resource_group_name = azurerm_resource_group.current.name
  location            = azurerm_resource_group.current.location
  allocation_method   = "Static"
}

resource "azurerm_user_assigned_identity" "application-gateway" {
  resource_group_name = azurerm_resource_group.current.name
  location            = azurerm_resource_group.current.location
  name                = "${var.global_prefix}-${local.gateway_prefix}-identity"
}

resource "azurerm_key_vault_access_policy" "application-gateway" {
  key_vault_id = azurerm_key_vault.current.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = azurerm_user_assigned_identity.application-gateway.principal_id

  certificate_permissions = [
    "Get",
    "List",
  ]

  key_permissions = [
    "Get",
    "List",
  ]

  secret_permissions = [
    "Get",
    "List",
  ]
}

resource "azurerm_application_gateway" "this" {
  name                = "${var.global_prefix}-${local.gateway_prefix}"
  resource_group_name = azurerm_resource_group.current.name
  location            = azurerm_resource_group.current.location

  sku {
    name     = "Basic"
    tier     = "Basic"
    capacity = 1
  }

  identity {
    type         = "UserAssigned"
    identity_ids = [azurerm_user_assigned_identity.application-gateway.id]
  }

  ssl_certificate {
    name                = local.certificate_name
    key_vault_secret_id = azurerm_key_vault_certificate.certificates["${var.global_prefix}-certificate"].versionless_secret_id
  }

  gateway_ip_configuration {
    name      = local.gateway_ip_configuration_name
    subnet_id = azurerm_subnet.application-gateway.id
  }

  frontend_port {
    name = local.frontend_port_name
    port = 443
  }

  frontend_ip_configuration {
    name                 = local.frontend_ip_configuration_name
    public_ip_address_id = azurerm_public_ip.application-gateway.id
  }

  backend_address_pool {
    name  = local.backend_address_pool_name
    fqdns = [azurerm_linux_web_app.identity-api.default_hostname]
  }

  probe {
    name                                      = local.probe_name
    protocol                                  = "Http"
    path                                      = "/healthcheck"
    interval                                  = 240
    timeout                                   = 30
    unhealthy_threshold                       = 3
    pick_host_name_from_backend_http_settings = true
    match {
      body        = "Healthy"
      status_code = [200]
    }
  }

  backend_http_settings {
    name                                = local.backend_http_settings_name
    cookie_based_affinity               = "Disabled"
    port                                = 80
    protocol                            = "Http"
    probe_name                          = local.probe_name
    pick_host_name_from_backend_address = true
  }

  http_listener {
    name                           = local.http_listener_name
    frontend_ip_configuration_name = local.frontend_ip_configuration_name
    frontend_port_name             = local.frontend_port_name
    protocol                       = "Https"
    ssl_certificate_name           = local.certificate_name
  }

  request_routing_rule {
    name                       = local.routing_rule_name
    priority                   = 9
    rule_type                  = "Basic"
    http_listener_name         = local.http_listener_name
    backend_address_pool_name  = local.backend_address_pool_name
    backend_http_settings_name = local.backend_http_settings_name
  }
}