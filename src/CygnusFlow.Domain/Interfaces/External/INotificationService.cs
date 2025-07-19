using CygnusFlow.Domain.DTOs;
using CygnusFlow.Domain.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Interfaces.External
{
    public interface INotificationService
    {
        Task<Result<bool>> EnviarNotificacaoAsync(int usuarioId, string titulo, string mensagem, string tipo);
        Task<Result<bool>> EnviarNotificacaoGrupoAsync(List<int> usuarioIds, string titulo, string mensagem, string tipo);
        Task<ResultList<NotificacaoDto>> ObterNotificacoesUsuarioAsync(int usuarioId, bool apenasNaoLidas = false);
        Task<Result<bool>> MarcarComoLidaAsync(int notificacaoId);
        Task<Result<int>> ContarNaoLidasAsync(int usuarioId);
    }
}
