using CygnusFlow.Domain.Entities;
using CygnusFlow.Domain.Interfaces.Repositories;
using CygnusFlow.Domain.Shared;
using CygnusFlow.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygnusFlow.Infrastructure.Data.Repositories
{
    public class ModuloSistemaRepository : IModuloSistemaRepository
    {
        private readonly CygnusFlowContext _context;

        public ModuloSistemaRepository(CygnusFlowContext context)
        {
            _context = context;
        }

        public Task<Result<int>> ContarProjetosAsync(int moduloId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ModuloSistema>> CreateAsync(ModuloSistema modulo)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> ExisteNomeAsync(string nome, int? ignorarId = null)
        {
            throw new NotImplementedException();
        }

        public Task<ResultList<ModuloSistema>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<ModuloSistema>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ModuloSistema>> UpdateAsync(ModuloSistema modulo)
        {
            throw new NotImplementedException();
        }
    }
}
