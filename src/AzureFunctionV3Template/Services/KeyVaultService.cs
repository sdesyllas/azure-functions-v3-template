using AzureFunctionV3Template.Configurations;
using AzureFunctionV3Template.Helpers;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace AzureFunctionV3Template.Services
{
    /// <summary>
    /// Keyvault service with Manager Identity connection (no cliendid, secret)
    /// https://docs.microsoft.com/en-us/azure/app-service/overview-managed-identity?tabs=dotnet
    /// 
    /// It's using the system-assigned identity when deployed in azure
    /// It's using Visual Studio account that has access to the keyvault resource when running locally 
    /// </summary>
    public class KeyVaultService : IVaultService
    {
        private FunctionConfiguration _configuration;
        private ILogger<KeyVaultService> _logger;
        private readonly KeyVaultClient _keyVaultClient;

        public KeyVaultService(IOptions<FunctionConfiguration> messageResponderConfiguration, ILogger<KeyVaultService> logger)
        {
            _logger = logger;
            _configuration = messageResponderConfiguration.Value;

            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            _keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
        }

        public async Task<string> GetSecretAsync(string secret)
        {
            var secretPath = $"https://{Environment.GetEnvironmentVariable(Constants.KeyVaultKeyName)}.vault.azure.net/secrets/{secret}";
            string kvSecret;
            try
            {
                var kvResponse = await _keyVaultClient.GetSecretAsync(secretPath);
                kvSecret = kvResponse.Value;
                _logger.LogDebug($"{_configuration.ServiceName} - fetched secret from {secretPath}");
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                kvSecret = string.Empty;
            }
            return kvSecret;
        }
    }
}
