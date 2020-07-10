using System.Threading.Tasks;

namespace AzureFunctionV3Template.Services
{
    public interface IVaultService
    {
        Task<string> GetSecretAsync(string secret);
    }
}
