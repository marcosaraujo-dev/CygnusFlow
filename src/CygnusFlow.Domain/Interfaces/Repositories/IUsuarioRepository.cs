using CygnusFlow.Domain.Entities;
using CygnusFlow.Domain.Shared;
using CygnusFlow.Domain.Specifications;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Result<Usuario>> GetByIdAsync(int id);
        Task<Result<Usuario>> GetByEmailAsync(string email);
        Task<ResultList<Usuario>> GetAllAsync(UsuarioFiltro? filtro = null);
        Task<Result<Usuario>> CreateAsync(Usuario usuario);
        Task<Result<Usuario>> UpdateAsync(Usuario usuario);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<bool>> ExisteEmailAsync(string email, int? ignorarId = null);
        Task<Result<bool>> AlterarSenhaAsync(int usuarioId, string novaSenhaHash);
        Task<ResultList<Usuario>> GetByEquipeAsync(int equipeId);
        Task<ResultList<Usuario>> GetByTipoAsync(int tipoUsuario);
    }


}
