terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=4.54.0"
    }
    time = {
      source  = "hashicorp/time"
      version = "~>0.12"
    }
    null = {
      source  = "hashicorp/null"
      version = "~>3.2"
    }
    external = {
      source  = "hashicorp/external"
      version = "~>2.3"
    }
    acme = {
      source  = "vancluever/acme"
      version = "~>2.32"
    }
    azuread = {
      source  = "hashicorp/azuread"
      version = "~> 3.1.0"
    }
    random = {
      source  = "hashicorp/random"
      version = "~> 3.7"
    }
    azapi = {
      source  = "Azure/azapi"
      version = "~> 1.14.0"
    }
  }

  required_version = ">= 0.12"
}

provider "azurerm" {
  features {}
  subscription_id = var.subscription_id
}

provider "acme" {
  server_url = var.acme_server_url
}

provider "azuread" {
  tenant_id = data.azurerm_client_config.current.tenant_id
}
