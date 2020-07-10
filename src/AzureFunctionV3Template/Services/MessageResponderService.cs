using AzureFunctionV3Template.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace AzureFunctionV3Template.Services
{
    public class MessageResponderService : IMessageResponderService
    {
        private FunctionConfiguration _messageResponderConfiguration;
        private ILogger<MessageResponderService> _logger;
        private readonly IVaultService _vaultService;

        public MessageResponderService(IOptions<FunctionConfiguration> messageResponderConfiguration, ILogger<MessageResponderService> logger, IVaultService vaultService)
        {
            _messageResponderConfiguration = messageResponderConfiguration.Value;
            _logger = logger;
            _vaultService = vaultService;
        }

        public string GetPositiveMessage()
        {
            _logger.LogInformation("Very Positive!");
            return _messageResponderConfiguration.PositiveResponseMessage;
        }

        public string GetNegativeMessage()
        {
            _logger.LogInformation("Very negative!");
            return _messageResponderConfiguration.NegativeResponseMessage;
        }

        public async Task<string> GetSecretMessage()
        {
            var secret = await _vaultService.GetSecretAsync("MessageResponder-ASecret");
            return secret;
        }
    }
}