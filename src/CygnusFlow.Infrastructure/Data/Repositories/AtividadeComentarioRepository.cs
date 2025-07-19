using CygnusFlow.Domain.Entities;
using CygnusFlow.Domain.Interfaces.Repositories;
using CygnusFlow.Domain.Shared;
using CygnusFlow.Domain.Specifications;
using CygnusFlow.Infrastructure.Data.Context;
using System;
using System.Threading.Tasks;

namespace CygnusFlow.Infrastructure.Data.Repositories
{
    public class AtividadeComentarioRepository: IAtividadeComentarioRepository
    {
        private readonly CygnusFlowContext _context;

        public AtividadeComentarioRepository(CygnusFlowContext context)
        {
            _context = context;
        }

        public Task<Result<AtividadeComentario>> CreateAsync(AtividadeComentario comentario)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultList<AtividadeComentario>> GetByAtividadeIdAsync(int atividadeId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<AtividadeComentario>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultList<AtividadeComentario>> GetByUsuarioIdAsync(int usuarioId)
        {
            throw new NotImplementedException();
        }

        public Task<ResultList<AtividadeComentario>> GetComentariosRecentesAsync(int limite = 10)
        {
            throw new NotImplementedException();
        }

        public Task<Result<AtividadeComentario>> UpdateAsync(AtividadeComentario comentario)
        {
            throw new NotImplementedException();
        }
    }
}
