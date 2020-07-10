using System.Threading.Tasks;

namespace AzureFunctionV3Template.Services
{
    public interface IMessageResponderService
    {
        string GetPositiveMessage();

        string GetNegativeMessage();

        Task<string> GetSecretMessage();
    }
}