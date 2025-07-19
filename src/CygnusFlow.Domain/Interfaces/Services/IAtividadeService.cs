using CygnusFlow.Domain.Entities;
using CygnusFlow.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Interfaces.Services
{
    public interface IAtividadeService
    {
        Task<NotificationResult> ValidarDependenciasAsync(Atividade atividade);
        Task<Result<TimeSpan>> CalcularTempoTrabalhadoAsync(int atividadeId);
        Task<NotificationResult> ValidarMudancaStatusAsync(Atividade atividade, int novoStatus);
        Task<Result<List<Atividade>>> ObterAtividadesDependentesAsync(int atividadeId);
        Task<NotificationResult> ValidarExclusaoAsync(int atividadeId);
    }
}
