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
                var model = await _context.Atividades
                    .Include(a => a.Projeto)
                    .Include(a => a.Responsavel)
                    .Include(a => a.TipoAtividade)
                    .Include(a => a.StatusProjeto)
                    .Include(a => a.Comentarios)
                        .ThenInclude(c => c.Usuario)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (model == null)
                    return Result<Atividade>.Failure("Id", ErrorMessages.GetMessage(ErrorCodes.NOT_FOUND, "Atividade"));

                var atividade = AtividadeMapper.ToDomain(model);
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
                var model = await _context.Atividades
                    .Include(a => a.Projeto)
                    .Include(a => a.Responsavel)
                    .Include(a => a.TipoAtividade)
                    .Include(a => a.StatusProjeto)
                    .FirstOrDefaultAsync(a => a.Codigo == codigo);

                if (model == null)
                    return Result<Atividade>.Failure("Codigo", ErrorMessages.GetMessage(ErrorCodes.NOT_FOUND, "Atividade"));

                var atividade = AtividadeMapper.ToDomain(model);
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
                var models = await _context.Atividades
                    .Include(a => a.Responsavel)
                    .Include(a => a.TipoAtividade)
                    .Include(a => a.StatusProjeto)
                    .Where(a => a.ProjetoId == projetoId)
                    .OrderByDescending(a => a.DataCadastro)
                    .ToListAsync();

                var atividades = models.Select(AtividadeMapper.ToDomain).ToList();
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
                var models = await _context.Atividades
                    .Include(a => a.Projeto)
                    .Include(a => a.TipoAtividade)
                    .Include(a => a.StatusProjeto)
                    .Where(a => a.ResponsavelId == responsavelId)
                    .OrderByDescending(a => a.DataCadastro)
                    .ToListAsync();

                var atividades = models.Select(AtividadeMapper.ToDomain).ToList();
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
                    .Include(a => a.TipoAtividade)
                    .Include(a => a.StatusProjeto)
                    .AsQueryable();

                // Aplicar filtros
                if (!string.IsNullOrEmpty(filtros.Nome))
                    query = query.Where(a => a.Nome.Contains(filtros.Nome));

                if (filtros.ProjetoId.HasValue)
                    query = query.Where(a => a.ProjetoId == filtros.ProjetoId.Value);

                if (filtros.ResponsavelId.HasValue)
                    query = query.Where(a => a.ResponsavelId == filtros.ResponsavelId.Value);

                if (filtros.TipoAtividadeId.HasValue)
                    query = query.Where(a => a.TipoAtividadeId == filtros.TipoAtividadeId.Value);

                if (filtros.StatusId.HasValue)
                    query = query.Where(a => a.StatusProjetoId == filtros.StatusId.Value);

                if (filtros.DataInicio.HasValue)
                    query = query.Where(a => a.DataInicioPlanejada >= filtros.DataInicio.Value);

                if (filtros.DataFim.HasValue)
                    query = query.Where(a => a.DataFimPlanejada <= filtros.DataFim.Value);

                var models = await query
                    .OrderByDescending(a => a.DataCadastro)
                    .ToListAsync();

                var atividades = models.Select(AtividadeMapper.ToDomain).ToList();
                return ResultList<Atividade>.Success(atividades);
            }
            catch (Exception ex)
            {
                var notification = new NotificationResult();
                notification.AddError("Database", $"Erro ao buscar atividades: {ex.Message}");
                return ResultList<Atividade>.Failure(notification);
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
                var hoje = DateTime.Now.Date;
                var models = await _context.Atividades
                    .Include(a => a.Projeto)
                    .Include(a => a.Responsavel)
                    .Include(a => a.TipoAtividade)
                    .Include(a => a.StatusProjeto)
                    .Where(a => a.DataFimPlanejada.HasValue &&
                               a.DataFimPlanejada.Value < hoje &&
                               a.StatusProjetoId != 3) // 3 = Concluído
                    .OrderByDescending(a => a.DataCadastro)
                    .ToListAsync();

                var atividades = models.Select(AtividadeMapper.ToDomain).ToList();
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
                var models = await _context.Atividades
                    .Include(a => a.Projeto)
                    .Include(a => a.Responsavel)
                    .Include(a => a.TipoAtividade)
                    .Include(a => a.StatusProjeto)
                    .Where(a =>
                        (a.DataInicioPlanejada >= dataInicio && a.DataInicioPlanejada <= dataFim) ||
                        (a.DataFimPlanejada >= dataInicio && a.DataFimPlanejada <= dataFim))
                    .OrderByDescending(a => a.DataCadastro)
                    .ToListAsync();

                var atividades = models.Select(AtividadeMapper.ToDomain).ToList();
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
                var count = await _context.Atividades.CountAsync(a => a.StatusProjetoId == statusId);
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
                if (!validation.IsValid)
                    return Result<Atividade>.Failure(validation);

                var model = AtividadeMapper.ToModel(atividade);
                _context.Atividades.Add(model);
                await _context.SaveChangesAsync();

                // Atualizar entidade com ID gerado
                atividade.CarregarDados(
                    model.Id, atividade.Codigo, atividade.Nome, atividade.ProjetoId,
                    atividade.ResponsavelId, atividade.TipoAtividadeId, atividade.DataInicioPlanejada,
                    atividade.DataFimPlanejada, atividade.DataInicioReal, atividade.DataFimReal,
                    atividade.StatusProjetoId, atividade.Observacoes, atividade.Impedimentos,
                    atividade.DataCadastro
                );

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
                if (!validation.IsValid)
                    return Result<Atividade>.Failure(validation);

                var model = await _context.Atividades.FindAsync(atividade.Id);
                if (model == null)
                    return Result<Atividade>.Failure("Id", ErrorMessages.GetMessage(ErrorCodes.NOT_FOUND, "Atividade"));

                AtividadeMapper.UpdateModel(model, atividade);
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
                var model = await _context.Atividades.FindAsync(id);
                if (model == null)
                    return Result<bool>.Failure("Id", ErrorMessages.GetMessage(ErrorCodes.NOT_FOUND, "Atividade"));

                _context.Atividades.Remove(model);
                await _context.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Database", $"Erro ao excluir atividade: {ex.Message}");
            }
        }

        public async Task<Result<Atividade>> GetAtividadeAtrasadaAsync(int id)
        {
            try
            {
                var hoje = DateTime.Now.Date;
                var model = await _context.Atividades
                    .Include(a => a.Projeto)
                    .Include(a => a.Responsavel)
                    .Include(a => a.TipoAtividade)
                    .Include(a => a.StatusProjeto)
                    .FirstOrDefaultAsync(a => a.Id == id &&
                                            a.DataFimPlanejada.HasValue &&
                                            a.DataFimPlanejada.Value < hoje &&
                                            a.StatusProjetoId != 3);

                if (model == null)
                    return Result<Atividade>.Failure("Id", ErrorMessages.GetMessage(ErrorCodes.NOT_FOUND, "Atividade atrasada"));

                var atividade = AtividadeMapper.ToDomain(model);
                return Result<Atividade>.Success(atividade);
            }
            catch (Exception ex)
            {
                return Result<Atividade>.Failure("Database", $"Erro ao buscar atividade atrasada: {ex.Message}");
            }
        }
    }
}
