using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using TimerTrigger.StartupExtensions;

[assembly: FunctionsStartup(typeof(TimerTrigger.Startup))]

namespace TimerTrigger
{
    class Startup : FunctionsStartup
    {
        public IConfigurationRoot _configurationRoot; 


        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            FunctionsHostBuilderContext context = builder.GetContext();
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


            var azureServiceTokeProvider = new AzureServiceTokenProvider();

            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokeProvider.KeyVaultTokenCallback));


            _configurationRoot = builder.ConfigurationBuilder
                                    .AddJsonFile(Path.Combine(context.ApplicationRootPath, "appsettings.json"), optional: true, reloadOnChange: false)
                                    .AddJsonFile(Path.Combine(context.ApplicationRootPath, $"appsettings.{environment}.json"), optional: true, reloadOnChange: false)
                                    .AddEnvironmentVariables()
                                    .Build();

            builder.ConfigurationBuilder.AddAzureKeyVault($"https://{_configurationRoot["dataconnections:azurekeyvault:keyvaultname"]}.vault.azure.net/", keyVaultClient, new DefaultKeyVaultSecretManager()).Build();

            _configurationRoot= builder.ConfigurationBuilder.Build();
        }



        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<Function1>();
            builder.Services.AddApplicationInsightLogging(_configurationRoot);
        }
    }
}
