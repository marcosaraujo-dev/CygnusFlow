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
    public class ProjetoRepository : IProjetoRepository
    {
        private readonly CygnusFlowContext _context;

        public ProjetoRepository(CygnusFlowContext context)
        {
            _context = context;
        }

        public async Task<Result<Projeto>> CreateAsync(Projeto projeto)
        {
            try
            {
                var model = ProjetoMapper.ToModel(projeto);
                _context.Projetos.Add(model);
                await _context.SaveChangesAsync();

                // Atualizar entidade com ID gerado
                projeto.CarregarDados(
                    model.Id, projeto.Codigo, projeto.Nome, projeto.ModuloId,
                    projeto.CriticidadeId, projeto.DataInicioPO, projeto.DataFimPO,
                    projeto.EstimativaHoras, projeto.StatusProjetoId, projeto.DataCadastro
                );

                return Result<Projeto>.Success(projeto);
            }
            catch (Exception ex)
            {
                return Result<Projeto>.Failure("Database", $"Erro ao criar projeto: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            try
            {
                var model = await _context.Projetos.FindAsync(id);
                if (model == null)
                    return Result<bool>.Failure("Projeto", "Projeto não encontrado");

                _context.Projetos.Remove(model);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Database", $"Erro ao excluir projeto: {ex.Message}");
            }
        }

        public async Task<Result<bool>> ExisteCodigoAsync(string codigo, int? ignorarId = null)
        {
            try
            {
                var exists = await _context.Projetos
                    .AnyAsync(p => p.Codigo == codigo && (ignorarId == null || p.Id != ignorarId));

                return Result<bool>.Success(exists);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure("Database", $"Erro ao verificar código: {ex.Message}");
            }
        }

        public async Task<Result<Projeto>> GetByCodigoAsync(string codigo)
        {
            try
            {
                var model = await _context.Projetos
                    .Include(p => p.Modulo)
                    .Include(p => p.Criticidade)
                    .Include(p => p.StatusProjeto)
                    .Include(p => p.Atividades)
                        .ThenInclude(a => a.Responsavel)
                    .Include(p => p.Comentarios)
                    .FirstOrDefaultAsync(p => p.Codigo == codigo);

                if (model == null)
                    return Result<Projeto>.Failure("Projeto", "Projeto não encontrado");

                var projeto = ProjetoMapper.ToDomain(model);
                return Result<Projeto>.Success(projeto);
            }
            catch (Exception ex)
            {
                return Result<Projeto>.Failure("Database", $"Erro ao buscar projeto: {ex.Message}");
            }
        }

        public async Task<Result<Projeto>> GetByIdAsync(int id)
        {
            try
            {
                var model = await _context.Projetos
                    .Include(p => p.Modulo)
                    .Include(p => p.Criticidade)
                    .Include(p => p.StatusProjeto)
                    .Include(p => p.Atividades)
                        .ThenInclude(a => a.Responsavel)
                    .Include(p => p.Comentarios)
                        .ThenInclude(c => c.Usuario)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (model == null)
                    return Result<Projeto>.Failure("Projeto", "Projeto não encontrado");

                var projeto = ProjetoMapper.ToDomain(model);
                return Result<Projeto>.Success(projeto);
            }
            catch (Exception ex)
            {
                return Result<Projeto>.Failure("Database", $"Erro ao buscar projeto: {ex.Message}");
            }
        }

        public async Task<ResultList<Projeto>> GetByFiltrosAsync(ProjetoFiltro filtros)
        {
            try
            {
                var query = _context.Projetos
                    .Include(p => p.Modulo)
                    .Include(p => p.Criticidade)
                    .Include(p => p.StatusProjeto)
                    .AsQueryable();

                // Aplicar filtros
                if (!string.IsNullOrWhiteSpace(filtros.Codigo))
                    query = query.Where(p => p.Codigo.Contains(filtros.Codigo));

                if (!string.IsNullOrWhiteSpace(filtros.Nome))
                    query = query.Where(p => p.Nome.Contains(filtros.Nome));

                if (filtros.ModuloId.HasValue)
                    query = query.Where(p => p.ModuloId == filtros.ModuloId.Value);

                if (filtros.CriticidadeId.HasValue)
                    query = query.Where(p => p.CriticidadeId == filtros.CriticidadeId.Value);

               

                if (filtros.DataInicio.HasValue)
                    query = query.Where(p => p.DataInicioPO >= filtros.DataInicio.Value);

                if (filtros.DataFim.HasValue)
                    query = query.Where(p => p.DataInicioPO <= filtros.DataFim.Value);

              

                if (filtros.ApenasAtrasados == true)
                {
                    var hoje = DateTime.Now.Date;
                    query = query.Where(p => p.DataFimPO < hoje && p.StatusProjetoId != 3);
                }

                // Contar total
                var totalCount = await query.CountAsync();

                // Aplicar ordenação
                //query = ApplyOrdering(query, filtros.OrderBy, filtros.OrderDescending);

                // Aplicar paginação
                var models = await query
                    .Skip((filtros.Pagina - 1) * filtros.TamanhoPagina)
                    .Take(filtros.TamanhoPagina)
                    .ToListAsync();

                var projetos = models.Select(ProjetoMapper.ToDomain).ToList();
                return ResultList<Projeto>.Success(projetos, totalCount);
            }
            catch (Exception ex)
            {
                var notification = new NotificationResult();
                notification.AddError("Database", $"Erro ao buscar projetos: {ex.Message}");
                return ResultList<Projeto>.Failure(notification);
            }
        }

        public async Task<ResultList<Projeto>> GetProjetosAtrasadosAsync()
        {
            try
            {
                var hoje = DateTime.Now.Date;
                var models = await _context.Projetos
                    .Include(p => p.Modulo)
                    .Include(p => p.Criticidade)
                    .Include(p => p.StatusProjeto)
                    .Where(p => p.DataFimPO < hoje && p.StatusProjetoId != 3)
                    .ToListAsync();

                var projetos = models.Select(ProjetoMapper.ToDomain).ToList();
                return ResultList<Projeto>.Success(projetos);
            }
            catch (Exception ex)
            {
                var notification = new NotificationResult();
                notification.AddError("Database", $"Erro ao buscar projetos atrasados: {ex.Message}");
                return ResultList<Projeto>.Failure(notification);
            }
        }

        public async Task<ResultList<Projeto>> GetProjetosPorModuloAsync(int moduloId)
        {
            try
            {
                var models = await _context.Projetos
                    .Include(p => p.Modulo)
                    .Include(p => p.Criticidade)
                    .Include(p => p.StatusProjeto)
                    .Where(p => p.ModuloId == moduloId)
                    .ToListAsync();

                var projetos = models.Select(ProjetoMapper.ToDomain).ToList();
                return ResultList<Projeto>.Success(projetos);
            }
            catch (Exception ex)
            {
                var notification = new NotificationResult();
                notification.AddError("Database", $"Erro ao buscar projetos por módulo: {ex.Message}");
                return ResultList<Projeto>.Failure(notification);
            }
        }

        public async Task<ResultList<Projeto>> GetProjetosPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                var models = await _context.Projetos
                    .Include(p => p.Modulo)
                    .Include(p => p.Criticidade)
                    .Include(p => p.StatusProjeto)
                    .Where(p => p.DataInicioPO >= dataInicio && p.DataFimPO <= dataFim)
                    .ToListAsync();

                var projetos = models.Select(ProjetoMapper.ToDomain).ToList();
                return ResultList<Projeto>.Success(projetos);
            }
            catch (Exception ex)
            {
                var notification = new NotificationResult();
                notification.AddError("Database", $"Erro ao buscar projetos por período: {ex.Message}");
                return ResultList<Projeto>.Failure(notification);
            }
        }

        public async Task<Result<int>> GetProximoNumeroAsync()
        {
            try
            {
                var ultimoProjeto = await _context.Projetos
                    .Where(p => p.Codigo.StartsWith("PRJ-"))
                    .OrderByDescending(p => p.Codigo)
                    .FirstOrDefaultAsync();

                if (ultimoProjeto == null)
                    return Result<int>.Success(1);

                // Extrair número do código PRJ-00001
                var numeroStr = ultimoProjeto.Codigo.Substring(4);
                if (int.TryParse(numeroStr, out var numero))
                    return Result<int>.Success(numero + 1);

                return Result<int>.Success(1);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Database", $"Erro ao obter próximo número: {ex.Message}");
            }
        }

        public async Task<Result<Projeto>> UpdateAsync(Projeto projeto)
        {
            try
            {
                var model = await _context.Projetos.FindAsync(projeto.Id);
                if (model == null)
                    return Result<Projeto>.Failure("Projeto", "Projeto não encontrado");

                ProjetoMapper.UpdateModel(model, projeto);
                await _context.SaveChangesAsync();

                return Result<Projeto>.Success(projeto);
            }
            catch (Exception ex)
            {
                return Result<Projeto>.Failure("Database", $"Erro ao atualizar projeto: {ex.Message}");
            }
        }

        public async Task<Result<int>> ContarProjetosPorStatusAsync(int statusId)
        {
            try
            {
                var count = await _context.Projetos.CountAsync(p => p.StatusProjetoId == statusId);
                return Result<int>.Success(count);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure("Database", $"Erro ao contar projetos: {ex.Message}");
            }
        }

        private IQueryable<Models.ProjetoModel> ApplyOrdering(IQueryable<Models.ProjetoModel> query, string? orderBy, bool descending)
        {
            return orderBy?.ToLower() switch
            {
                "codigo" => descending ? query.OrderByDescending(p => p.Codigo) : query.OrderBy(p => p.Codigo),
                "nome" => descending ? query.OrderByDescending(p => p.Nome) : query.OrderBy(p => p.Nome),
                "datafinpo" => descending ? query.OrderByDescending(p => p.DataFimPO) : query.OrderBy(p => p.DataFimPO),
                "modulo" => descending ? query.OrderByDescending(p => p.Modulo.Nome) : query.OrderBy(p => p.Modulo.Nome),
                "status" => descending ? query.OrderByDescending(p => p.StatusProjeto.Nome) : query.OrderBy(p => p.StatusProjeto.Nome),
                _ => descending ? query.OrderByDescending(p => p.DataCadastro) : query.OrderBy(p => p.DataCadastro)
            };
        }
    }
}
