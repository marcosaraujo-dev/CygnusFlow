using CygnusFlow.Domain.Entities;
using CygnusFlow.Domain.Shared;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<NotificationResult> ValidarPermissoesAsync(Usuario usuario, string acao);
        Task<Result<string>> GerarCodigoRedefinicaoAsync(int usuarioId);
        Task<NotificationResult> ValidarSenhaAsync(string senha);
        Task<Result<string>> GerarSenhaTemporariaAsync();
        Task<NotificationResult> ValidarAlteracaoSenhaAsync(int usuarioId, string senhaAtual, string novaSenha);
        Task<Result<bool>> UsuarioPodeAcessarProjetoAsync(int usuarioId, int projetoId);
    }
}
