using CygnusFlow.Domain.Entities;
using CygnusFlow.Domain.Shared;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Interfaces.Repositories
{
    public interface IProjetoComentarioRepository
    {
        Task<Result<ProjetoComentario>> GetByIdAsync(int id);
        Task<ResultList<ProjetoComentario>> GetByProjetoIdAsync(int projetoId);
        Task<ResultList<ProjetoComentario>> GetByUsuarioIdAsync(int usuarioId);
        Task<Result<ProjetoComentario>> CreateAsync(ProjetoComentario comentario);
        Task<Result<ProjetoComentario>> UpdateAsync(ProjetoComentario comentario);
        Task<Result<bool>> DeleteAsync(int id);
        Task<ResultList<ProjetoComentario>> GetComentariosRecentesAsync(int limite = 10);
    }


}
