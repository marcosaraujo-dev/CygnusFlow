using CygnusFlow.Domain.Entities;
using CygnusFlow.Domain.Interfaces.Services;
using CygnusFlow.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygnusFlow.Application.Services
{
    public class AtividadeService : IAtividadeService
    {
        public Task<Result<TimeSpan>> CalcularTempoTrabalhadoAsync(int atividadeId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<Atividade>>> ObterAtividadesDependentesAsync(int atividadeId)
        {
            throw new NotImplementedException();
        }

        public Task<NotificationResult> ValidarDependenciasAsync(Atividade atividade)
        {
            throw new NotImplementedException();
        }

        public Task<NotificationResult> ValidarExclusaoAsync(int atividadeId)
        {
            throw new NotImplementedException();
        }

        public Task<NotificationResult> ValidarMudancaStatusAsync(Atividade atividade, int novoStatus)
        {
            throw new NotImplementedException();
        }
    }
}
