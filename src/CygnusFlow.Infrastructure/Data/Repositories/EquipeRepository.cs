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
    public class EquipeRepository : IEquipeRepository
    {
        private readonly CygnusFlowContext _context;

        public EquipeRepository(CygnusFlowContext context)
        {
            _context = context;
        }

        public Task<Result<int>> ContarMembrosAsync(int equipeId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Equipe>> CreateAsync(Equipe equipe)
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

        public Task<ResultList<Equipe>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<Equipe>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<Equipe>> UpdateAsync(Equipe equipe)
        {
            throw new NotImplementedException();
        }
    }
}
