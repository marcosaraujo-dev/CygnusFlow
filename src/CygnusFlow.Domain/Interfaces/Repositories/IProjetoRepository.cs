using CygnusFlow.Domain.Entities;
using CygnusFlow.Domain.Shared;
using CygnusFlow.Domain.Specifications;
using System;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Interfaces.Repositories
{
    public interface IProjetoRepository
    {
        Task<Result<Projeto>> GetByIdAsync(int id);
        Task<Result<Projeto>> GetByCodigoAsync(string codigo);
        Task<ResultList<Projeto>> GetByFiltrosAsync(ProjetoFiltro filtros);
        Task<Result<Projeto>> CreateAsync(Projeto projeto);
        Task<Result<Projeto>> UpdateAsync(Projeto projeto);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<bool>> ExisteCodigoAsync(string codigo, int? ignorarId = null);
        Task<Result<int>> GetProximoNumeroAsync();
        Task<ResultList<Projeto>> GetProjetosAtrasadosAsync();
        Task<ResultList<Projeto>> GetProjetosPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<Result<int>> ContarProjetosPorStatusAsync(int statusId);
        Task<ResultList<Projeto>> GetProjetosPorModuloAsync(int moduloId);
    }


}
