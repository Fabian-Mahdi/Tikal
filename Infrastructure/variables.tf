variable "global_prefix" {
  type        = string
  description = "Prefix added to the names of all deployed resources"
}

variable "resource_group_location" {
  type        = string
  description = "Name of the region where the resource group will be deployed"
}

variable "domain_name" {
  type        = string
  description = "Name of the domain used by the system"
}

variable "acme_email" {
  type        = string
  description = "Email with which to register to acme"
}

variable "subscription_id" {
  type        = string
  description = "Id of the currently active subscription"
}

variable "acme_server_url" {
  type        = string
  description = "Url of the acme server used to sign certificates"
}

variable "db_admin_login" {
  type        = string
  description = "Username for the database server"
}

variable "identity_db_name" {
  type        = string
  description = "Name of the database used by the identity api"
}

variable "tikal_db_name" {
  type        = string
  description = "Name of the database used by the tikal backend"
}

variable "identity_api_image" {
  type        = string
  description = "The docker image (including tag) which is to be deployed in the identity api app service"
}

variable "tikal_image" {
  type        = string
  description = "The docker image (including tag) which is to be deployed in the tikal backend app service"
}

variable "frontend_image" {
  type        = string
  description = "The docker image (including tag) which is to be deployed in the frontend app service"
}

variable "vpn_application_id" {
  type        = string
  description = "The application id of the Azure VPN Enterprise application"
}

variable "github_database_id" {
  type        = string
  description = "The database id of your github organization"
}
