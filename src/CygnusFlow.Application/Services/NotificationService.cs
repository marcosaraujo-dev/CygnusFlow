using CygnusFlow.Domain.DTOs;
using CygnusFlow.Domain.Interfaces.External;
using CygnusFlow.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygnusFlow.Application.Services
{
    public class NotificationService : INotificationService
    {
        public Task<Result<int>> ContarNaoLidasAsync(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> EnviarNotificacaoAsync(int usuarioId, string titulo, string mensagem, string tipo)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> EnviarNotificacaoGrupoAsync(List<int> usuarioIds, string titulo, string mensagem, string tipo)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> MarcarComoLidaAsync(int notificacaoId)
        {
            throw new NotImplementedException();
        }

        public Task<ResultList<NotificacaoDto>> ObterNotificacoesUsuarioAsync(int usuarioId, bool apenasNaoLidas = false)
        {
            throw new NotImplementedException();
        }
    }
}
