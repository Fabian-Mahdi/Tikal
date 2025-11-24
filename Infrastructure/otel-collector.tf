locals {
  otel_collector_prefix = "otel-collector"
}

resource "azurerm_storage_account" "aci_storage" {
  name                     = "tikalotelcollector"
  resource_group_name      = azurerm_resource_group.current.name
  location                 = azurerm_resource_group.current.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  min_tls_version          = "TLS1_2"
}

resource "azurerm_storage_share" "container_share" {
  name               = "${var.global_prefix}-${local.otel_collector_prefix}-storage-share"
  storage_account_id = azurerm_storage_account.aci_storage.id
  quota              = 50
  enabled_protocol   = "SMB"
}

resource "azurerm_storage_share_file" "otel_config_file" {
  name              = "config.yaml"
  storage_share_url = azurerm_storage_share.container_share.url
  source            = "./otel-collector/config.yaml"
}

resource "azurerm_subnet" "otel-container-group" {
  name                 = "${var.global_prefix}-${local.otel_collector_prefix}-group-subnet"
  resource_group_name  = azurerm_resource_group.current.name
  virtual_network_name = azurerm_virtual_network.root.name
  address_prefixes     = ["10.123.12.0/24"]

  delegation {
    name = "aci-delegation"
    service_delegation {
      name = "Microsoft.ContainerInstance/containerGroups"
      actions = [
        "Microsoft.Network/virtualNetworks/subnets/action",
      ]
    }
  }
}

resource "azurerm_container_group" "otel-containergroup" {
  name                = "${var.global_prefix}-${local.otel_collector_prefix}-container-group"
  location            = azurerm_resource_group.current.location
  resource_group_name = azurerm_resource_group.current.name
  ip_address_type     = "Private"
  os_type             = "Linux"

  container {
    name  = "otel-collector"
    image = "otel/opentelemetry-collector-contrib"

    cpu    = "0.2"
    memory = "0.5"

    ports {
      port     = 4317
      protocol = "TCP"
    }
    ports {
      port     = 4318
      protocol = "TCP"
    }
    ports {
      port     = 13133
      protocol = "TCP"
    }

    environment_variables = {
      "GRAFANA_CLOUD_INSTANCE_ID"   = var.grafana_cloud_instance_id,
      "GRAFANA_CLOUD_API_KEY"       = var.grafana_cloud_api_key,
      "GRAFANA_CLOUD_OTLP_ENDPOINT" = var.grafana_cloud_otlp_endpoint,
    }

    volume {
      name                 = "config"
      mount_path           = "/etc/otelcol-contrib"
      storage_account_name = azurerm_storage_account.aci_storage.name
      storage_account_key  = azurerm_storage_account.aci_storage.primary_access_key
      share_name           = azurerm_storage_share.container_share.name
    }
  }

  subnet_ids = [azurerm_subnet.otel-container-group.id]
}
