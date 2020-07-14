using AzureFunctionV3Template.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AzureFunctionV3Template
{
    public class MessageFunction
    {
        private IMessageResponderService _messageResponderService;

        public MessageFunction(IMessageResponderService messageResponderService)
        {
            _messageResponderService = messageResponderService;
        }

        [FunctionName("positive")]
        public async Task<IActionResult> GetPositiveMessage(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("MessageFunction - C# HTTP trigger function processed a request for GetPositiveMessage.");

            return new OkObjectResult(_messageResponderService.GetPositiveMessage());
        }

        [FunctionName("negative")]
        public async Task<IActionResult> GetNegativeMessage(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("MessageFunction - C# HTTP trigger function processed a request for GetNegativeMessage.");

            return new OkObjectResult(_messageResponderService.GetNegativeMessage());
        }

        [FunctionName("secret")]
        public async Task<IActionResult> GetSecretMessage(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
        ILogger log)
        {
            log.LogInformation("MessageFunction - C# HTTP trigger function processed a request for GetSecretMessage.");

            return new OkObjectResult(await _messageResponderService.GetSecretMessage());
        }
    }
}