using CygnusFlow.Domain.Entities;
using CygnusFlow.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Interfaces.Services
{
    public interface IProjetoService
    {
        Task<NotificationResult> ValidarPrazosAsync(Projeto projeto);
        Task<Result<TimeSpan>> CalcularAtrasoAsync(Projeto projeto);
        Task<NotificationResult> VerificarConflitosAsync(Projeto projeto);
        Task<Result<double>> CalcularProgressoAsync(int projetoId);
        Task<NotificationResult> ValidarExclusaoAsync(int projetoId);
        Task<Result<List<Projeto>>> SugerirProjetosSimilares(Projeto projeto);
    }
    }
