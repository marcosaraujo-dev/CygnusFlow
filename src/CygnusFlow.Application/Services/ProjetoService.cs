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
    public class ProjetoService : IProjetoService
    {
        public Task<Result<TimeSpan>> CalcularAtrasoAsync(Projeto projeto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<double>> CalcularProgressoAsync(int projetoId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<Projeto>>> SugerirProjetosSimilares(Projeto projeto)
        {
            throw new NotImplementedException();
        }

        public Task<NotificationResult> ValidarExclusaoAsync(int projetoId)
        {
            throw new NotImplementedException();
        }

        public Task<NotificationResult> ValidarPrazosAsync(Projeto projeto)
        {
            throw new NotImplementedException();
        }

        public Task<NotificationResult> VerificarConflitosAsync(Projeto projeto)
        {
            throw new NotImplementedException();
        }
    }
}
