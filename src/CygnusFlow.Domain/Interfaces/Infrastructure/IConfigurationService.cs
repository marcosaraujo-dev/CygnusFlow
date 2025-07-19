using CygnusFlow.Domain.DTOs;
using CygnusFlow.Domain.Shared;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Interfaces.Infrastructure
{
    public interface IConfigurationService
    {
        Task<Result<string>> GetConfigurationAsync(string chave);
        Task<Result<T>> GetConfigurationAsync<T>(string chave);
        Task<Result<bool>> SetConfigurationAsync(string chave, object valor);
        Task<Result<bool>> RemoveConfigurationAsync(string chave);
        Task<ResultList<ConfiguracaoDto>> GetAllConfigurationsAsync();
        Task<Result<bool>> ResetToDefaultsAsync();
    }
}
