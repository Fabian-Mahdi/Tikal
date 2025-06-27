resource "azurerm_service_plan" "current" {
  name                = "${var.global_prefix}-app-service-plan"
  resource_group_name = azurerm_resource_group.current.name
  location            = azurerm_resource_group.current.location
  os_type             = "Linux"
  sku_name            = "B1"
}