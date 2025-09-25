locals {
  tikal_backend_prefix = "tikal-backend"
}

resource "azurerm_subnet" "tikal-backend-endpoint" {
  name                              = "${var.global_prefix}-${local.tikal_backend_prefix}-endpoint-subnet"
  resource_group_name               = azurerm_resource_group.current.name
  virtual_network_name              = azurerm_virtual_network.root.name
  address_prefixes                  = ["10.123.11.0/24"]
  private_endpoint_network_policies = "Enabled"
}

resource "azurerm_private_endpoint" "tikal-backend" {
  name                = "${var.global_prefix}-${local.tikal_backend_prefix}-private-endpoint"
  location            = azurerm_resource_group.current.location
  resource_group_name = azurerm_resource_group.current.name
  subnet_id           = azurerm_subnet.tikal-backend-endpoint.id

  private_dns_zone_group {
    name                 = "${var.global_prefix}-${local.tikal_backend_prefix}-private-dns-zone-group"
    private_dns_zone_ids = [azurerm_private_dns_zone.dns_private.id]
  }

  private_service_connection {
    name                           = "${var.global_prefix}-${local.tikal_backend_prefix}-private-endpoint-connection"
    private_connection_resource_id = azurerm_linux_web_app.tikal-backend.id
    subresource_names              = ["sites"]
    is_manual_connection           = false
  }
}

resource "azurerm_linux_web_app" "tikal-backend" {
  name                      = "${var.global_prefix}-${local.tikal_backend_prefix}"
  resource_group_name       = azurerm_resource_group.current.name
  location                  = azurerm_service_plan.current.location
  service_plan_id           = azurerm_service_plan.current.id
  virtual_network_subnet_id = azurerm_subnet.services.id

  site_config {
    ip_restriction_default_action = "Deny"

    ip_restriction {
      action = "Allow"

      service_tag = "AzureContainerRegistry"
    }

    container_registry_use_managed_identity       = true
    container_registry_managed_identity_client_id = azurerm_user_assigned_identity.tikal-backend.client_id

    application_stack {
      docker_image_name   = var.tikal_image
      docker_registry_url = "https://${azurerm_container_registry.current.login_server}"
    }
  }

  identity {
    type         = "UserAssigned"
    identity_ids = [azurerm_user_assigned_identity.tikal-backend.id]
  }

  app_settings = {
    DOCKER_ENABLE_CI = "true"
    AZURE_CLIENT_ID  = azurerm_user_assigned_identity.tikal-backend.client_id
  }
}

resource "azurerm_user_assigned_identity" "tikal-backend" {
  resource_group_name = azurerm_resource_group.current.name
  location            = azurerm_resource_group.current.location
  name                = "${var.global_prefix}-${local.tikal_backend_prefix}-identity"
}

resource "azurerm_key_vault_access_policy" "tikal-backend" {
  key_vault_id = azurerm_key_vault.tikal.id
  tenant_id    = data.azurerm_client_config.current.tenant_id
  object_id    = azurerm_user_assigned_identity.tikal-backend.principal_id

  secret_permissions = [
    "Get",
    "List"
  ]
}

resource "azurerm_role_assignment" "tikal-backend-arc-pull" {
  scope                = data.azurerm_subscription.current.id
  role_definition_name = "AcrPull"
  principal_id         = azurerm_user_assigned_identity.tikal-backend.principal_id
}

resource "azurerm_key_vault_secret" "tikal_mediatr_license" {
  key_vault_id = azurerm_key_vault.tikal.id
  name         = "Mediatr--LicenseKey"
  value        = var.mediatr_license
}
