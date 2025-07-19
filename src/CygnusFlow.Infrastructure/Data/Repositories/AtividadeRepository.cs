using CygnusFlow.Domain.Constants;
using CygnusFlow.Domain.Entities;
using CygnusFlow.Domain.Interfaces.Repositories;
using CygnusFlow.Domain.Shared;
using CygnusFlow.Domain.Specifications;
using CygnusFlow.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CygnusFlow.Infrastructure.Data.Repositories
{
    public class AtividadeRepository : IAtividadeRepository
    {
        private readonly CygnusFlowContext _context;

        public AtividadeRepository(CygnusFlowContext context)
        {
            _context = context;
        }

        public async Task<Result<Atividade>> GetByIdAsync(int id)
        {
            try
            {
                var atividade = await _context.Atividades
                    .Include(a => a.Projeto)
                    .Include(a => a.Responsavel)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (atividade == null)
                    return Result<Atividade>.Failure("Id", ErrorMessages.GetMessage(ErrorCodes.NOT_FOUND, "Atividade"));

                return Result<Atividade>.Success(atividade);
            }
            catch (Exception ex)
            {
                return Result<Atividade>.Failure("Database", $"Erro ao buscar atividade: {ex.Message}");
            }
        }

        public async Task<Result<Atividade>> GetByCodigoAsync(string codigo)
        {
            try
            {
                var atividade = await _context.Atividades
                    .Include(a => a.Projeto)
                    .Include(a => a.Responsavel)
                    .FirstOrDefaultAsync(a => a.Codigo == codigo);

                if (atividade == null)
                    return Result<Atividade>.Failure("Codigo", ErrorMessages.GetMessage(ErrorCodes.NOT_FOUND, "Atividade"));

                return Result<Atividade>.Success(atividade);
            }
            catch (Exception ex)
            {
                return Result<Atividade>.Failure("Database", $"Erro ao buscar atividade: {ex.Message}");
            }
        }

        public async Task<ResultList<Atividade>> GetByProjetoIdAsync(int projetoId)
        {
            try
            {
                var atividades = await _context.Atividades
                    .Include(a => a.Responsavel)
                    .Where(a => a.ProjetoId == projetoId)
                    .OrderByDescending(a => a.DataCadastro)
                    .ToListAsync();

                return ResultList<Atividade>.Success(atividades);
            }
            catch (Exception ex)
            {
                var notification = new NotificationResult();
                notification.AddError("Database", $"Erro ao buscar atividades: {ex.Message}");
                return ResultList<Atividade>.Failure(notification);
            }
        }

        public async Task<ResultList<Atividade>> GetByResponsavelAsync(int responsavelId)
        {
            try
            {
                var atividades = await _context.Atividades
                    .Include(a => a.Projeto)
                    .Where(a => a.ResponsavelId == responsavelId)
                    .OrderByDescending(a => a.DataCadastro)
                    .ToListAsync();

                return ResultList<Atividade>.Success(atividades);
            }
            catch (Exception ex)
            {
                var notification = new NotificationResult();
                notification.AddError("Database", $"Erro ao buscar atividades: {ex.Message}");
                return ResultList<Atividade>.Failure(notification);
            }
        }

        public async Task<ResultList<Atividade>> GetByFiltrosAsync(AtividadeFiltro filtros)
        {
            try
            {
                var query = _context.Atividades
                    .Include(a => a.Projeto)
                    .Include(a => a.Responsavel)
                    .AsQueryable();

                // Aplicar filtros
                if (!string.IsNullOrEmpty(filtros.Nome))
                    query = query.Where(a => a.Nome.Contains(filtros.Nome));

                if (filtros.ProjetoId.HasValue)
                    query = query.Where(a => a.ProjetoId == filtros.ProjetoId.Value);

                if (filtros.ResponsavelId.HasValue)
                    query = query.Where(a => a.ResponsavelId == filtros.ResponsavelId.Value);

                if (filtros.TipoAtividadeId.HasValue)
                    query = query.Where(a => (int)a.TipoAtividadeId == filtros.TipoAtividadeId.Value);

                if (filtros.StatusId.HasValue)
                    query = query.Where(a => (int)a.StatusProjetoId == filtros.StatusId.Value);

                if (filtros.DataInicio.HasValue)
                    query = query.Where(a => a.DataInicioPlanejada >= filtros.DataInicio.Value);

                if (filtros.DataFim.HasValue)
                    query = query.Where(a => a.DataFimPlanejada <= filtros.DataFim.Value);

                var atividades = await query
                    .OrderByDescending(a => a.DataCadastro)
                    .ToListAsync();

                return ResultList<Atividade>.Success(atividades);
            }
            catch (Exception ex)
            {
                var notification = new NotificationResult();
                notification.AddError("Database", $"Erro ao buscar atividades: {ex.Message}");
                return ResultList<Atividade>.Failure(notification);
            }
        }
        public async Task<Result<Atividade>> GetAtividadeAtrasadaAsync(int id)
        {
            try
            {
                var atividade = await _context.Atividades
                    .Include(a => a.Projeto)
                    .Include(a => a.Responsavel)
                    .FirstOrDefaultAsync(a => a.Id == id && a.EstaAtrasada());
                if (atividade == null)
                    return Result<Atividade>.Failure("Id", ErrorMessages.GetMessage(ErrorCodes.NOT_FOUND, "Atividade atrasada"));
                return Result<Atividade>.Success(atividade);
            }
            catch (Exception ex)
            {
                return Result<Atividade>.Failure("Database", $"Erro ao buscar atividade atrasada: {ex.Message}");
            }
        }

        public async Task<Result<bool>> ExisteCodigoAsync(string codigo, int? ignorarId = null)
        {
            try
            {
                var query = _context.Atividades.AsQueryable();
                if (ignorarId.HasValue)
                    query = query.Where(a => a.Id != ignorarId.Value);

                var exists = await query.AnyAsync(a => a.Codigo == codigo);
                return Result<bool>.Success(exists);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Database", $"Erro ao verificar código: {ex.Message}");
            }
        }

        public async Task<Result<int>> GetProximoNumeroAsync()
        {
            try
            {
                var ultimo = await _context.Atividades
                    .OrderByDescending(a => a.Id)
                    .Select(a => a.Id)
                    .FirstOrDefaultAsync();

                return Result<int>.Success(ultimo + 1);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Database", $"Erro ao obter próximo número: {ex.Message}");
            }
        }

        public async Task<ResultList<Atividade>> GetAtividadesAtrasadasAsync()
        {
            try
            {
                var atividades = await _context.Atividades
                    .Include(a => a.Projeto)
                    .Include(a => a.Responsavel)
                    .Where(a => a.EstaAtrasada())
                    .OrderByDescending(a => a.DataCadastro)
                    .ToListAsync();

                return ResultList<Atividade>.Success(atividades);
            }
            catch (Exception ex)
            {
                var notification = new NotificationResult();
                notification.AddError("Database", $"Erro ao buscar atividades atrasadas: {ex.Message}");
                return ResultList<Atividade>.Failure(notification);
            }
        }

        public async Task<ResultList<Atividade>> GetAtividadesPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                var atividades = await _context.Atividades
                    .Include(a => a.Projeto)
                    .Include(a => a.Responsavel)
                    .Where(a =>
                        (a.DataInicioPlanejada >= dataInicio && a.DataInicioPlanejada <= dataFim) ||
                        (a.DataFimPlanejada >= dataInicio && a.DataFimPlanejada <= dataFim))
                    .OrderByDescending(a => a.DataCadastro)
                    .ToListAsync();

                return ResultList<Atividade>.Success(atividades);
            }
            catch (Exception ex)
            {
                var notification = new NotificationResult();
                notification.AddError("Database", $"Erro ao buscar atividades por período: {ex.Message}");
                return ResultList<Atividade>.Failure(notification);
            }
        }

        public async Task<Result<int>> ContarAtividadesPorStatusAsync(int statusId)
        {
            try
            {
                var count = await _context.Atividades.CountAsync(a => (int)a.StatusProjetoId == statusId);
                return Result<int>.Success(count);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Database", $"Erro ao contar atividades por status: {ex.Message}");
            }

        }

        public async Task<Result<Atividade>> CreateAsync(Atividade atividade)
        {
            try
            {
                var validation = atividade.Validate();
                if (validation.Errors.Any())
                    return Result<Atividade>.Failure("DataBase", ErrorMessages.GetMessage(ErrorCodes.DATABASE_ERROR, "Atividade"));

                await _context.Atividades.AddAsync(atividade);
                await _context.SaveChangesAsync();

                return Result<Atividade>.Success(atividade);
            }
            catch (Exception ex)
            {
                return Result<Atividade>.Failure("Database", $"Erro ao criar atividade: {ex.Message}");
            }
        }


        public async Task<Result<Atividade>> UpdateAsync(Atividade atividade)
        {
            try
            {
                var validation = atividade.Validate();
                if (validation.Errors.Any())
                    return Result<Atividade>.Failure(validation);

                var existing = await _context.Atividades.FindAsync(atividade.Id);
                if (existing == null)
                    return Result<Atividade>.Failure("Id", ErrorMessages.GetMessage(ErrorCodes.NOT_FOUND, "Atividade"));

                _context.Entry(existing).CurrentValues.SetValues(atividade);
                await _context.SaveChangesAsync();

                return Result<Atividade>.Success(atividade);
            }
            catch (Exception ex)
            {
                return Result<Atividade>.Failure("Database", $"Erro ao atualizar atividade: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            try
            {
                var atividade = await _context.Atividades.FindAsync(id);
                if (atividade == null)
                    return Result<bool>.Failure("Id", ErrorMessages.GetMessage(ErrorCodes.NOT_FOUND, "Atividade"));

                _context.Atividades.Remove(atividade);
                await _context.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Database", $"Erro ao excluir atividade: {ex.Message}");
            }
        }

    }
    }
