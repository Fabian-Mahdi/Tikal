locals {
  frontend_prefix = "frontend"
}

resource "azurerm_subnet" "frontend" {
  name                 = "${var.global_prefix}-${local.frontend_prefix}-subnet"
  resource_group_name  = azurerm_resource_group.current.name
  virtual_network_name = azurerm_virtual_network.root.name
  address_prefixes     = ["10.123.6.0/24"]

  delegation {
    name = "${var.global_prefix}-${local.frontend_prefix}-subnet-delegation"

    service_delegation {
      name    = "Microsoft.Web/serverFarms"
      actions = ["Microsoft.Network/virtualNetworks/subnets/action"]
    }
  }
}

resource "azurerm_subnet" "frontend-endpoint" {
  name                              = "${var.global_prefix}-${local.frontend_prefix}-endpoint-subnet"
  resource_group_name               = azurerm_resource_group.current.name
  virtual_network_name              = azurerm_virtual_network.root.name
  address_prefixes                  = ["10.123.7.0/24"]
  private_endpoint_network_policies = "Enabled"
}

resource "azurerm_private_endpoint" "frontend" {
  name                = "${var.global_prefix}-${local.frontend_prefix}-private-endpoint"
  location            = azurerm_resource_group.current.location
  resource_group_name = azurerm_resource_group.current.name
  subnet_id           = azurerm_subnet.frontend-endpoint.id

  private_dns_zone_group {
    name                 = "${var.global_prefix}-${local.frontend_prefix}-private-dns-zone-group"
    private_dns_zone_ids = [azurerm_private_dns_zone.dns_private.id]
  }

  private_service_connection {
    name                           = "${var.global_prefix}-${local.frontend_prefix}-private-endpoint-connection"
    private_connection_resource_id = azurerm_linux_web_app.frontend.id
    subresource_names              = ["sites"]
    is_manual_connection           = false
  }
}

resource "azurerm_linux_web_app" "frontend" {
  name                      = "${var.global_prefix}-${local.frontend_prefix}"
  resource_group_name       = azurerm_resource_group.current.name
  location                  = azurerm_service_plan.current.location
  service_plan_id           = azurerm_service_plan.current.id
  virtual_network_subnet_id = azurerm_subnet.frontend.id

  site_config {
    ip_restriction_default_action = "Deny"

    ip_restriction {
      action = "Allow"

      service_tag = "AzureContainerRegistry"
    }

    container_registry_use_managed_identity       = true
    container_registry_managed_identity_client_id = azurerm_user_assigned_identity.frontend.client_id

    application_stack {
      docker_image_name   = var.frontend_image
      docker_registry_url = "https://${azurerm_container_registry.current.login_server}"
    }
  }

  identity {
    type         = "UserAssigned"
    identity_ids = [azurerm_user_assigned_identity.frontend.id]
  }

  app_settings = {
    DOCKER_ENABLE_CI = "true"
    AZURE_CLIENT_ID  = azurerm_user_assigned_identity.frontend.client_id
  }
}

resource "azurerm_user_assigned_identity" "frontend" {
  resource_group_name = azurerm_resource_group.current.name
  location            = azurerm_resource_group.current.location
  name                = "${var.global_prefix}-${local.frontend_prefix}-identity"
}

resource "azurerm_role_assignment" "frontend-arc-pull" {
  scope                = data.azurerm_subscription.current.id
  role_definition_name = "AcrPull"
  principal_id         = azurerm_user_assigned_identity.frontend.principal_id
}