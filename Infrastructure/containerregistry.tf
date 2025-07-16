resource "azurerm_container_registry" "current" {
  name                = "${var.global_prefix}ContainerRegistry"
  resource_group_name = azurerm_resource_group.current.name
  location            = azurerm_resource_group.current.location
  sku                 = "Standard"
  admin_enabled       = false
}

resource "azurerm_container_registry_webhook" "identity-api" {
  name                = "${var.global_prefix}IdentityApiWebhook"
  resource_group_name = azurerm_resource_group.current.name
  registry_name       = azurerm_container_registry.current.name
  location            = azurerm_resource_group.current.location

  service_uri = "https://${azurerm_linux_web_app.identity-api.site_credential.0.name}:${azurerm_linux_web_app.identity-api.site_credential.0.password}@${azurerm_linux_web_app.identity-api.name}.scm.azurewebsites.net/api/registry/webhook"
  status      = "enabled"
  scope       = var.identity_api_image
  actions     = ["push"]
}

resource "azurerm_container_registry_webhook" "frontend" {
  name                = "${var.global_prefix}FrontendWebhook"
  resource_group_name = azurerm_resource_group.current.name
  registry_name       = azurerm_container_registry.current.name
  location            = azurerm_resource_group.current.location

  service_uri = "https://${azurerm_linux_web_app.frontend.site_credential.0.name}:${azurerm_linux_web_app.frontend.site_credential.0.password}@${azurerm_linux_web_app.frontend.name}.scm.azurewebsites.net/api/registry/webhook"
  status      = "enabled"
  scope       = var.frontend_image
  actions     = ["push"]
}