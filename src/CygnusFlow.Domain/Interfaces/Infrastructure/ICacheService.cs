using CygnusFlow.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Interfaces.Infrastructure
{
    public interface ICacheService
    {
        Task<Result<T?>> GetAsync<T>(string key) where T : class;
        Task<Result<bool>> SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class;
        Task<Result<bool>> RemoveAsync(string key);
        Task<Result<bool>> ExistsAsync(string key);
        Task<Result<bool>> RemovePatternAsync(string pattern);
        Task<Result<List<string>>> GetKeysAsync(string pattern = "*");
        Task<Result<bool>> SetHashAsync(string key, Dictionary<string, object> values, TimeSpan? expiration = null);
        Task<Result<Dictionary<string, T>?>> GetHashAsync<T>(string key) where T : class;
    }
}
