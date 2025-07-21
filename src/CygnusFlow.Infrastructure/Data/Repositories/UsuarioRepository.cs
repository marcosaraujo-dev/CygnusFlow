using CygnusFlow.Domain.Constants;
using CygnusFlow.Domain.Entities;
using CygnusFlow.Domain.Interfaces.Repositories;
using CygnusFlow.Domain.Shared;
using CygnusFlow.Domain.Specifications;
using CygnusFlow.Infrastructure.Data.Context;
using CygnusFlow.Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygnusFlow.Infrastructure.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly CygnusFlowContext _context;

        public UsuarioRepository(CygnusFlowContext context)
        {
            _context = context;
        }

        public async Task<Result<Usuario>> GetByIdAsync(int id)
        {
            try
            {
                var model = await _context.Usuarios
                    .Include(u => u.Equipe)
                    .Include(u => u.TipoUsuario)
                    .Include(u => u.StatusUsuario)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (model == null)
                    return Result<Usuario>.Failure("Id", ErrorMessages.GetMessage(ErrorCodes.NOT_FOUND, "Usuário"));

                var usuario = UsuarioMapper.ToDomain(model);
                return Result<Usuario>.Success(usuario);
            }
            catch (Exception ex)
            {
                return Result<Usuario>.Failure("Database", $"Erro ao buscar usuário: {ex.Message}");
            }
        }

        public async Task<Result<Usuario>> GetByEmailAsync(string email)
        {
            try
            {
                var model = await _context.Usuarios
                    .Include(u => u.Equipe)
                    .Include(u => u.TipoUsuario)
                    .Include(u => u.StatusUsuario)
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (model == null)
                    return Result<Usuario>.Failure("Email", ErrorMessages.GetMessage(ErrorCodes.NOT_FOUND, "Usuário"));

                var usuario = UsuarioMapper.ToDomain(model);
                return Result<Usuario>.Success(usuario);
            }
            catch (Exception ex)
            {
                return Result<Usuario>.Failure("Database", $"Erro ao buscar usuário: {ex.Message}");
            }
        }

        public async Task<ResultList<Usuario>> GetAllAsync(UsuarioFiltro? filtro = null)
        {
            try
            {
                var query = _context.Usuarios
                    .Include(u => u.Equipe)
                    .Include(u => u.TipoUsuario)
                    .Include(u => u.StatusUsuario)
                    .AsQueryable();

                if (filtro != null)
                {
                    if (!string.IsNullOrEmpty(filtro.Nome))
                        query = query.Where(u => u.Nome.Contains(filtro.Nome));

                    if (!string.IsNullOrEmpty(filtro.Email))
                        query = query.Where(u => u.Email.Contains(filtro.Email));

                    if (filtro.EquipeId.HasValue)
                        query = query.Where(u => u.EquipeId == filtro.EquipeId.Value);

                    if (filtro.TipoUsuarioId.HasValue)
                        query = query.Where(u => u.TipoUsuarioId == filtro.TipoUsuarioId.Value);

                    if (filtro.StatusUsuarioId.HasValue)
                        query = query.Where(u => u.StatusUsuarioId == filtro.StatusUsuarioId.Value);
                }

                var totalCount = await query.CountAsync();

                var models = await query
                    .OrderBy(u => u.Nome)
                    .Skip(((filtro?.Pagina ?? 1) - 1) * (filtro?.TamanhoPagina ?? 50))
                    .Take(filtro?.TamanhoPagina ?? 50)
                    .ToListAsync();

                var usuarios = models.Select(UsuarioMapper.ToDomain).ToList();
                return ResultList<Usuario>.Success(usuarios, totalCount);
            }
            catch (Exception ex)
            {
                var notification = new NotificationResult();
                notification.AddError("Database", $"Erro ao buscar usuários: {ex.Message}");
                return ResultList<Usuario>.Failure(notification);
            }
        }

        public async Task<Result<Usuario>> CreateAsync(Usuario usuario)
        {
            try
            {
                var model = UsuarioMapper.ToModel(usuario);
                _context.Usuarios.Add(model);
                await _context.SaveChangesAsync();

                // Atualizar entidade com ID gerado
                usuario.Id = model.Id;

                return Result<Usuario>.Success(usuario);
            }
            catch (Exception ex)
            {
                return Result<Usuario>.Failure("Database", $"Erro ao criar usuário: {ex.Message}");
            }
        }

        public async Task<Result<Usuario>> UpdateAsync(Usuario usuario)
        {
            try
            {
                var model = await _context.Usuarios.FindAsync(usuario.Id);
                if (model == null)
                    return Result<Usuario>.Failure("Id", ErrorMessages.GetMessage(ErrorCodes.NOT_FOUND, "Usuário"));

                UsuarioMapper.UpdateModel(model, usuario);
                await _context.SaveChangesAsync();

                return Result<Usuario>.Success(usuario);
            }
            catch (Exception ex)
            {
                return Result<Usuario>.Failure("Database", $"Erro ao atualizar usuário: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            try
            {
                var model = await _context.Usuarios.FindAsync(id);
                if (model == null)
                    return Result<bool>.Failure("Id", ErrorMessages.GetMessage(ErrorCodes.NOT_FOUND, "Usuário"));

                _context.Usuarios.Remove(model);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Database", $"Erro ao excluir usuário: {ex.Message}");
            }
        }

        public async Task<Result<bool>> ExisteEmailAsync(string email, int? ignorarId = null)
        {
            try
            {
                var query = _context.Usuarios.Where(u => u.Email == email);

                if (ignorarId.HasValue)
                    query = query.Where(u => u.Id != ignorarId.Value);

                var existe = await query.AnyAsync();
                return Result<bool>.Success(existe);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Database", $"Erro ao verificar e-mail: {ex.Message}");
            }
        }

        public async Task<Result<bool>> AlterarSenhaAsync(int usuarioId, string novaSenhaHash)
        {
            try
            {
                var model = await _context.Usuarios.FindAsync(usuarioId);
                if (model == null)
                    return Result<bool>.Failure("Id", ErrorMessages.GetMessage(ErrorCodes.NOT_FOUND, "Usuário"));

                model.SenhaHash = novaSenhaHash;
                await _context.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Database", $"Erro ao alterar senha: {ex.Message}");
            }
        }

        public async Task<ResultList<Usuario>> GetByEquipeAsync(int equipeId)
        {
            try
            {
                var models = await _context.Usuarios
                    .Include(u => u.Equipe)
                    .Include(u => u.TipoUsuario)
                    .Include(u => u.StatusUsuario)
                    .Where(u => u.EquipeId == equipeId)
                    .OrderBy(u => u.Nome)
                    .ToListAsync();

                var usuarios = models.Select(UsuarioMapper.ToDomain).ToList();
                return ResultList<Usuario>.Success(usuarios);
            }
            catch (Exception ex)
            {
                var notification = new NotificationResult();
                notification.AddError("Database", $"Erro ao buscar usuários por equipe: {ex.Message}");
                return ResultList<Usuario>.Failure(notification);
            }
        }

        public async Task<ResultList<Usuario>> GetByTipoAsync(int tipoUsuario)
        {
            try
            {
                var models = await _context.Usuarios
                    .Include(u => u.Equipe)
                    .Include(u => u.TipoUsuario)
                    .Include(u => u.StatusUsuario)
                    .Where(u => u.TipoUsuarioId == tipoUsuario)
                    .OrderBy(u => u.Nome)
                    .ToListAsync();

                var usuarios = models.Select(UsuarioMapper.ToDomain).ToList();
                return ResultList<Usuario>.Success(usuarios);
            }
            catch (Exception ex)
            {
                var notification = new NotificationResult();
                notification.AddError("Database", $"Erro ao buscar usuários por tipo: {ex.Message}");
                return ResultList<Usuario>.Failure(notification);
            }
        }
    }
}