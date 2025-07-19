using CygnusFlow.Domain.Entities;
using CygnusFlow.Domain.Shared;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Interfaces.Repositories
{
    public interface IEquipeRepository
    {
        Task<Result<Equipe>> GetByIdAsync(int id);
        Task<ResultList<Equipe>> GetAllAsync();
        Task<Result<Equipe>> CreateAsync(Equipe equipe);
        Task<Result<Equipe>> UpdateAsync(Equipe equipe);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<bool>> ExisteNomeAsync(string nome, int? ignorarId = null);
        Task<Result<int>> ContarMembrosAsync(int equipeId);
    }


}
