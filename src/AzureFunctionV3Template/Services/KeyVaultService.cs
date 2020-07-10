using AzureFunctionDependencyInjection.Configurations;
using AzureFunctionDependencyInjection.Helpers;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace AzureFunctionDependencyInjection.Services
{
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
