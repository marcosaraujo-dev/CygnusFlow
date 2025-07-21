using CygnusFlow.Domain.Constants;
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
    public class ProjetoComentarioRepository : IProjetoComentarioRepository
    {
        private readonly CygnusFlowContext _context;

        public ProjetoComentarioRepository(CygnusFlowContext context)
        {
            _context = context;
        }

        public async Task<Result<ProjetoComentario>> GetByIdAsync(int id)
        {
            try
            {
                var model = await _context.ProjetoComentarios
                    .Include(pc => pc.Usuario)
                    .Include(pc => pc.Projeto)
                    .FirstOrDefaultAsync(pc => pc.Id == id);

                if (model == null)
                    return Result<ProjetoComentario>.Failure("Id", "Comentário não encontrado");

                var comentario = ProjetoComentarioMapper.ToDomain(model);
                return Result<ProjetoComentario>.Success(comentario);
            }
            catch (Exception ex)
            {
                return Result<ProjetoComentario>.Failure("Database", $"Erro ao buscar comentário: {ex.Message}");
            }
        }

        public async Task<ResultList<ProjetoComentario>> GetByProjetoIdAsync(int projetoId)
        {
            try
            {
                var models = await _context.ProjetoComentarios
                    .Include(pc => pc.Usuario)
                    .Where(pc => pc.ProjetoId == projetoId)
                    .OrderByDescending(pc => pc.DataComentario)
                    .ToListAsync();

                var comentarios = models.Select(ProjetoComentarioMapper.ToDomain).ToList();
                return ResultList<ProjetoComentario>.Success(comentarios);
            }
            catch (Exception ex)
            {
                var notification = new NotificationResult();
                notification.AddError("Database", $"Erro ao buscar comentários: {ex.Message}");
                return ResultList<ProjetoComentario>.Failure(notification);
            }
        }

        public async Task<ResultList<ProjetoComentario>> GetByUsuarioIdAsync(int usuarioId)
        {
            try
            {
                var models = await _context.ProjetoComentarios
                    .Include(pc => pc.Projeto)
                    .Where(pc => pc.UsuarioId == usuarioId)
                    .OrderByDescending(pc => pc.DataComentario)
                    .ToListAsync();

                var comentarios = models.Select(ProjetoComentarioMapper.ToDomain).ToList();
                return ResultList<ProjetoComentario>.Success(comentarios);
            }
            catch (Exception ex)
            {
                var notification = new NotificationResult();
                notification.AddError("Database", $"Erro ao buscar comentários: {ex.Message}");
                return ResultList<ProjetoComentario>.Failure(notification);
            }
        }

        public async Task<ResultList<ProjetoComentario>> GetComentariosRecentesAsync(int limite = 10)
        {
            try
            {
                var models = await _context.ProjetoComentarios
                    .Include(pc => pc.Usuario)
                    .Include(pc => pc.Projeto)
                    .OrderByDescending(pc => pc.DataComentario)
                    .Take(limite)
                    .ToListAsync();

                var comentarios = models.Select(ProjetoComentarioMapper.ToDomain).ToList();
                return ResultList<ProjetoComentario>.Success(comentarios);
            }
            catch (Exception ex)
            {
                var notification = new NotificationResult();
                notification.AddError("Database", $"Erro ao buscar comentários recentes: {ex.Message}");
                return ResultList<ProjetoComentario>.Failure(notification);
            }
        }

        public async Task<Result<ProjetoComentario>> CreateAsync(ProjetoComentario comentario)
        {
            try
            {
                var validation = comentario.Validate();
                if (!validation.IsValid)
                    return Result<ProjetoComentario>.Failure(validation);

                var model = ProjetoComentarioMapper.ToModel(comentario);
                _context.ProjetoComentarios.Add(model);
                await _context.SaveChangesAsync();

                comentario.Id = model.Id;
                return Result<ProjetoComentario>.Success(comentario);
            }
            catch (Exception ex)
            {
                return Result<ProjetoComentario>.Failure("Database", $"Erro ao criar comentário: {ex.Message}");
            }
        }

        public async Task<Result<ProjetoComentario>> UpdateAsync(ProjetoComentario comentario)
        {
            try
            {
                var validation = comentario.Validate();
                if (!validation.IsValid)
                    return Result<ProjetoComentario>.Failure(validation);

                var model = await _context.ProjetoComentarios.FindAsync(comentario.Id);
                if (model == null)
                    return Result<ProjetoComentario>.Failure("Id", "Comentário não encontrado");

                ProjetoComentarioMapper.UpdateModel(model, comentario);
                await _context.SaveChangesAsync();

                return Result<ProjetoComentario>.Success(comentario);
            }
            catch (Exception ex)
            {
                return Result<ProjetoComentario>.Failure("Database", $"Erro ao atualizar comentário: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            try
            {
                var model = await _context.ProjetoComentarios.FindAsync(id);
                if (model == null)
                    return Result<bool>.Failure("Id", "Comentário não encontrado");

                _context.ProjetoComentarios.Remove(model);
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
