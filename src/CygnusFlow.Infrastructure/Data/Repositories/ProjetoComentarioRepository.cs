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
    public class ProjetoComentarioRepository : IProjetoComentarioRepository
    {
        private readonly CygnusFlowContext _context;

        public ProjetoComentarioRepository(CygnusFlowContext context)
        {
            _context = context;
        }

        public Task<Result<ProjetoComentario>> CreateAsync(ProjetoComentario comentario)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ProjetoComentario>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultList<ProjetoComentario>> GetByProjetoIdAsync(int projetoId)
        {
            throw new NotImplementedException();
        }

        public Task<ResultList<ProjetoComentario>> GetByUsuarioIdAsync(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<ResultList<ProjetoComentario>> GetComentariosRecentesAsync(int limite = 10)
        {
            throw new NotImplementedException();
        }

        public Task<Result<ProjetoComentario>> UpdateAsync(ProjetoComentario comentario)
        {
            throw new NotImplementedException();
        }
    }
}
