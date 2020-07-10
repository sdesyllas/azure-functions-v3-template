using System.Threading.Tasks;

namespace AzureFunctionDependencyInjection.Services
{
    public interface IVaultService
    {
        Task<string> GetSecretAsync(string secret);
    }
}
