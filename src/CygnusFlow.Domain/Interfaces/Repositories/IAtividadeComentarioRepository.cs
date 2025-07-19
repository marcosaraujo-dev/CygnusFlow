using CygnusFlow.Domain.Entities;
using CygnusFlow.Domain.Shared;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Interfaces.Repositories
{
    public interface IAtividadeComentarioRepository
    {
        Task<Result<AtividadeComentario>> GetByIdAsync(int id);
        Task<ResultList<AtividadeComentario>> GetByAtividadeIdAsync(int atividadeId);
        Task<ResultList<AtividadeComentario>> GetByUsuarioIdAsync(int usuarioId);
        Task<Result<AtividadeComentario>> CreateAsync(AtividadeComentario comentario);
        Task<Result<AtividadeComentario>> UpdateAsync(AtividadeComentario comentario);
        Task<Result<bool>> DeleteAsync(int id);
        Task<ResultList<AtividadeComentario>> GetComentariosRecentesAsync(int limite = 10);
    }


}
