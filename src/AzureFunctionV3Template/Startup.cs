using AzureFunctionV3Template.Configurations;
using AzureFunctionV3Template.Helpers;
using AzureFunctionV3Template.Services;
using AzureFunctionV3Template;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Reflection.Metadata;

[assembly: FunctionsStartup(typeof(Startup))]

namespace AzureFunctionV3Template
{
    /// <summary>
    /// https://medium.com/@therealjordanlee/dependency-injection-in-azure-functions-v3-7148d0574dfc
    /// This is where the magic happens.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Registering Configurations (IOptions pattern)
            builder
                .Services
                .AddOptions<FunctionConfiguration>()
                .Configure<IConfiguration>((messageResponderSettings, configuration) =>
                {
                    configuration.GetSection("MessageResponder").Bind(messageResponderSettings);
                });


            // Registering Serilog provider
            var logger = new LoggerConfiguration().CreateLogger();
            builder.Services.AddLogging(lb => lb.AddSerilog(logger));

            // Registering services
            builder
                .Services
                .AddSingleton<IVaultService, KeyVaultService>()
                .AddSingleton<IMessageResponderService, MessageResponderService>();
        }
    }
}