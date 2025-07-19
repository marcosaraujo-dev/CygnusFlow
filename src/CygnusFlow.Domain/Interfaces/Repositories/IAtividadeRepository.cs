using CygnusFlow.Domain.Entities;
using CygnusFlow.Domain.Shared;
using CygnusFlow.Domain.Specifications;
using System;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Interfaces.Repositories
{
    public interface IAtividadeRepository
    {
        Task<Result<Atividade>> GetByIdAsync(int id);
        Task<Result<Atividade>> GetByCodigoAsync(string codigo);
        Task<ResultList<Atividade>> GetByProjetoIdAsync(int projetoId);
        Task<ResultList<Atividade>> GetByResponsavelAsync(int responsavelId);
        Task<ResultList<Atividade>> GetByFiltrosAsync(AtividadeFiltro filtros);
        Task<Result<Atividade>> CreateAsync(Atividade atividade);
        Task<Result<Atividade>> UpdateAsync(Atividade atividade);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<bool>> ExisteCodigoAsync(string codigo, int? ignorarId = null);
        Task<Result<int>> GetProximoNumeroAsync();
        Task<Result<Atividade>> GetAtividadeAtrasadaAsync(int id);
        Task<ResultList<Atividade>> GetAtividadesAtrasadasAsync();
        Task<ResultList<Atividade>> GetAtividadesPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<Result<int>> ContarAtividadesPorStatusAsync(int statusId);
    }


}
