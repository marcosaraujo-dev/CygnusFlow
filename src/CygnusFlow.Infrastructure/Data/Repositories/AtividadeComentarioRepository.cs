using CygnusFlow.Domain.Entities;
using CygnusFlow.Domain.Interfaces.Repositories;
using CygnusFlow.Domain.Shared;
using CygnusFlow.Domain.Specifications;
using CygnusFlow.Infrastructure.Data.Context;
using CygnusFlow.Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CygnusFlow.Infrastructure.Data.Repositories
{
    public class AtividadeComentarioRepository : IAtividadeComentarioRepository
    {
        private readonly CygnusFlowContext _context;

        public AtividadeComentarioRepository(CygnusFlowContext context)
        {
            _context = context;
        }

        public async Task<Result<AtividadeComentario>> GetByIdAsync(int id)
        {
            try
            {
                var model = await _context.AtividadeComentarios
                    .Include(ac => ac.Usuario)
                    .Include(ac => ac.Atividade)
                    .FirstOrDefaultAsync(ac => ac.Id == id);

                if (model == null)
                    return Result<AtividadeComentario>.Failure("Id", "Comentário não encontrado");

                var comentario = AtividadeComentarioMapper.ToDomain(model);
                return Result<AtividadeComentario>.Success(comentario);
            }
            catch (Exception ex)
            {
                return Result<AtividadeComentario>.Failure("Database", $"Erro ao buscar comentário: {ex.Message}");
            }
        }

        public async Task<ResultList<AtividadeComentario>> GetByAtividadeIdAsync(int atividadeId)
        {
            try
            {
                var models = await _context.AtividadeComentarios
                    .Include(ac => ac.Usuario)
                    .Where(ac => ac.AtividadeId == atividadeId)
                    .OrderByDescending(ac => ac.DataComentario)
                    .ToListAsync();

                var comentarios = models.Select(AtividadeComentarioMapper.ToDomain).ToList();
                return ResultList<AtividadeComentario>.Success(comentarios);
            }
            catch (Exception ex)
            {
                var notification = new NotificationResult();
                notification.AddError("Database", $"Erro ao buscar comentários: {ex.Message}");
                return ResultList<AtividadeComentario>.Failure(notification);
            }
        }

        public async Task<ResultList<AtividadeComentario>> GetByUsuarioIdAsync(int usuarioId)
        {
            try
            {
                var models = await _context.AtividadeComentarios
                    .Include(ac => ac.Atividade)
                    .Where(ac => ac.UsuarioId == usuarioId)
                    .OrderByDescending(ac => ac.DataComentario)
                    .ToListAsync();

                var comentarios = models.Select(AtividadeComentarioMapper.ToDomain).ToList();
                return ResultList<AtividadeComentario>.Success(comentarios);
            }
            catch (Exception ex)
            {
                var notification = new NotificationResult();
                notification.AddError("Database", $"Erro ao buscar comentários: {ex.Message}");
                return ResultList<AtividadeComentario>.Failure(notification);
            }
        }

        public async Task<ResultList<AtividadeComentario>> GetComentariosRecentesAsync(int limite = 10)
        {
            try
            {
                var models = await _context.AtividadeComentarios
                    .Include(ac => ac.Usuario)
                    .Include(ac => ac.Atividade)
                    .OrderByDescending(ac => ac.DataComentario)
                    .Take(limite)
                    .ToListAsync();

                var comentarios = models.Select(AtividadeComentarioMapper.ToDomain).ToList();
                return ResultList<AtividadeComentario>.Success(comentarios);
            }
            catch (Exception ex)
            {
                var notification = new NotificationResult();
                notification.AddError("Database", $"Erro ao buscar comentários recentes: {ex.Message}");
                return ResultList<AtividadeComentario>.Failure(notification);
            }
        }

        public async Task<Result<AtividadeComentario>> CreateAsync(AtividadeComentario comentario)
        {
            try
            {
                var validation = comentario.Validate();
                if (!validation.IsValid)
                    return Result<AtividadeComentario>.Failure(validation);

                var model = AtividadeComentarioMapper.ToModel(comentario);
                _context.AtividadeComentarios.Add(model);
                await _context.SaveChangesAsync();

                comentario.Id = model.Id;
                return Result<AtividadeComentario>.Success(comentario);
            }
            catch (Exception ex)
            {
                return Result<AtividadeComentario>.Failure("Database", $"Erro ao criar comentário: {ex.Message}");
            }
        }

        public async Task<Result<AtividadeComentario>> UpdateAsync(AtividadeComentario comentario)
        {
            try
            {
                var validation = comentario.Validate();
                if (!validation.IsValid)
                    return Result<AtividadeComentario>.Failure(validation);

                var model = await _context.AtividadeComentarios.FindAsync(comentario.Id);
                if (model == null)
                    return Result<AtividadeComentario>.Failure("Id", "Comentário não encontrado");

                AtividadeComentarioMapper.UpdateModel(model, comentario);
                await _context.SaveChangesAsync();

                return Result<AtividadeComentario>.Success(comentario);
            }
            catch (Exception ex)
            {
                return Result<AtividadeComentario>.Failure("Database", $"Erro ao atualizar comentário: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            try
            {
                var model = await _context.AtividadeComentarios.FindAsync(id);
                if (model == null)
                    return Result<bool>.Failure("Id", "Comentário não encontrado");

                _context.AtividadeComentarios.Remove(model);
                await _context.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Database", $"Erro ao excluir comentário: {ex.Message}");
            }
        }
    }
}
