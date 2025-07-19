using CygnusFlow.Domain.Entities;
using CygnusFlow.Domain.Shared;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Interfaces.Repositories
{
    public interface IModuloSistemaRepository
    {
        Task<Result<ModuloSistema>> GetByIdAsync(int id);
        Task<ResultList<ModuloSistema>> GetAllAsync();
        Task<Result<ModuloSistema>> CreateAsync(ModuloSistema modulo);
        Task<Result<ModuloSistema>> UpdateAsync(ModuloSistema modulo);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<bool>> ExisteNomeAsync(string nome, int? ignorarId = null);
        Task<Result<int>> ContarProjetosAsync(int moduloId);
    }


}
