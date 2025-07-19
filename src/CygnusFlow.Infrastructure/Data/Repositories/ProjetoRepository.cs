using CygnusFlow.Domain.Constants;
using CygnusFlow.Domain.Entities;
using CygnusFlow.Domain.Interfaces.Repositories;
using CygnusFlow.Domain.Shared;
using CygnusFlow.Domain.Specifications;
using CygnusFlow.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CygnusFlow.Infrastructure.Data.Repositories
{
    public class ProjetoRepository : IProjetoRepository
    {
        private readonly CygnusFlowContext _context;

        public ProjetoRepository(CygnusFlowContext context)
        {
            _context = context;
        }
        
        public Task<Result<int>> ContarProjetosPorStatusAsync(int statusId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Projeto>> CreateAsync(Projeto projeto)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> ExisteCodigoAsync(string codigo, int? ignorarId = null)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Projeto>> GetByCodigoAsync(string codigo)
        {
            throw new NotImplementedException();
        }

        public Task<ResultList<Projeto>> GetByFiltrosAsync(ProjetoFiltro filtros)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Projeto>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultList<Projeto>> GetProjetosAtrasadosAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResultList<Projeto>> GetProjetosPorModuloAsync(int moduloId)
        {
            throw new NotImplementedException();
        }

        public Task<ResultList<Projeto>> GetProjetosPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            throw new NotImplementedException();
        }

        public Task<Result<int>> GetProximoNumeroAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<Projeto>> UpdateAsync(Projeto projeto)
        {
            throw new NotImplementedException();
        }
    }
}
