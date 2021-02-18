# Hello DotNet Versioning

Explores resources to enable seamless workflow in an Continuous delivery pipeline.
Should cover:
1. Centralized management of Feature Flags with [Azure App Configuration](https://azure.microsoft.com/services/app-configuration);
2. Uses API Versioning as recommended by [Microsoft`s API Design Guidelines](https://docs.microsoft.com/en-us/azure/architecture/best-practices/api-design#uri-versioning);
3. {Not covered yet} Deploy with Canary strategy on Azure Kubernetes Services;

## Getting started

This app uses `dotnet secrets` so, run the following _cmdlet_ to add Azure App Configuration connection string to your local host:
```sh
$ cd HellowVersioning.Api
$ dotnet user-secrets set ConnectionStrings:AppConfig "<one _of_your_azure_app_configuration_connection_strings>" 
$ dotnet user-secrets set ConnectionStrings:ServiceBus "<one_of_your_azure_service_bus_connection_strings>"
$ dotnet user-secrets set AppConfigurationEventHandling:TopicName "<the_topic_name_that_will_receive_AppConfiguration_change_events>"
$ dotnet user-secrets set AppConfigurationEventHandling:ServiceBusSubscription "<the_service_bus_subscription_name_for_the_topic>"
```