using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Hosting;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HelloVersioning.Api
{
    public class Program
    {
        private static IConfigurationRefresher _refresher = null;

        protected Program() { }

        public static void Main(string[] args)
        {
            var build = CreateHostBuilder(args).Build();
            var configuration = build.Services.GetService(typeof(IConfiguration)) as IConfiguration;
            RegisterRefreshEventHandler(configuration);
            build.Run();
        }

        private static void RegisterRefreshEventHandler(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ServiceBus");
            var topicName = configuration.GetValue<string>("AppConfigurationEventHandling:TopicName");
            var serviceBusSubscription = configuration.GetValue<string>("AppConfigurationEventHandling:ServiceBusSubscription");

            var serviceBusClient = new SubscriptionClient(connectionString, topicName, serviceBusSubscription);
            serviceBusClient.RegisterMessageHandler((message, cancellationToken) => {
                var messageText = Encoding.UTF8.GetString(message.Body);
                var messageData = JsonDocument.Parse(messageText).RootElement.GetProperty("data");
                var key = messageData.GetProperty("key").GetString();
                Console.WriteLine($"Event received for Key: {key}");
                _refresher.SetDirty();
                return Task.CompletedTask;
            }, (exceptionArgs) => {
                Console.WriteLine($"{exceptionArgs.Exception}");
                return Task.CompletedTask;
            });
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.ConfigureAppConfiguration(config => {
                        var settings = config.Build();
                        var connection = settings.GetConnectionString("AppConfig");
                        config.AddAzureAppConfiguration(options => {
                            options
                                .UseFeatureFlags(featureFlagOptions =>
                                    featureFlagOptions.CacheExpirationInterval = TimeSpan.FromMinutes(1))
                                .ConfigureRefresh(refreshConfiguration => 
                                    refreshConfiguration
                                        .Register("Beta")
                                        .SetCacheExpiration(TimeSpan.FromDays(1)))
                                .Connect(connection);
                            _refresher = options.GetRefresher();
                        });
                    }).UseStartup<Startup>());
    }
}
