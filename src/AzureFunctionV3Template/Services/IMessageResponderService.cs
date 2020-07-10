using System.Threading.Tasks;

namespace AzureFunctionDependencyInjection.Services
{
    public interface IMessageResponderService
    {
        string GetPositiveMessage();

        string GetNegativeMessage();

        Task<string> GetSecretMessage();
    }
}