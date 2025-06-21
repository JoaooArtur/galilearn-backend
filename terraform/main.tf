provider "azurerm" {
  features {}
  subscription_id = "001b430d-5995-44e9-aa06-0b25ccf361b0"
}

resource "azurerm_resource_group" "rg" {
  name     = "rg-galilearn"
  location = "brazilsouth"
}

resource "azurerm_container_app_environment" "env" {
  name                = "cae-galilearn"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
}

resource "azurerm_container_app" "app" {
  name                         = "galilearn-api"
  container_app_environment_id = azurerm_container_app_environment.env.id
  resource_group_name          = azurerm_resource_group.rg.name
  revision_mode                = "Single"

  template {
    container {
      name   = "galilearn-api"
      image  = "acrgalilearn.azurecr.io/galilearn-api:latest"
      cpu    = 0.5
      memory = "1.0Gi"
    }
  }

  ingress {
    external_enabled = true
    target_port      = 80
    traffic_weight {
      percentage      = 100
      latest_revision = true
    }
  }
}