using CygnusFlow.Domain.DTOs;
using CygnusFlow.Domain.Shared;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Interfaces.Infrastructure
{
    public interface IAuditService
    {
        Task<Result<bool>> RegistrarAcaoAsync(int usuarioId, string acao, string entidade, int entidadeId, string detalhes = "");
        Task<Result<bool>> RegistrarLoginAsync(int usuarioId, string ipAddress, bool sucesso);
        Task<Result<bool>> RegistrarAlteracaoAsync(int usuarioId, string entidade, int entidadeId, object valorAnterior, object valorNovo);
        Task<ResultList<AuditLogDto>> ObterLogsAsync(AuditFiltro filtro);
        Task<Result<bool>> LimparLogsAntigosAsync(int diasParaManter = 90);
    }
}
