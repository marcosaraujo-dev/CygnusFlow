using CygnusFlow.Application.DTOs.Dashboard;
using CygnusFlow.Domain.Enums;
using CygnusFlow.Domain.Interfaces.Repositories;
using CygnusFlow.Domain.Shared;
using CygnusFlow.Domain.Specifications;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CygnusFlow.Application.Services
{
    public class DashboardService
    {
        private readonly IProjetoRepository _projetoRepository;
        private readonly IAtividadeRepository _atividadeRepository;

        public DashboardService(IProjetoRepository projetoRepository, IAtividadeRepository atividadeRepository)
        {
            _projetoRepository = projetoRepository;
            _atividadeRepository = atividadeRepository;
        }

        public async Task<Result<DashboardDto>> GetDashboardDataAsync()
        {
            var filtro = new ProjetoFiltro();
            var projetosResult = await _projetoRepository.GetByFiltrosAsync(filtro);

            if (!projetosResult.IsSuccess)
                return Result<DashboardDto>.Failure(projetosResult.Notifications);

            var projetos = projetosResult.Items;

            var dashboard = new DashboardDto
            {
                ProjetosAtivos = projetos.Count(p => p.StatusProjetoId == (int) StatusProjeto.EmAndamento),
                ProjetosAtrasados = projetos.Count(p => p.EstaAtrasado),
                ProjetosConcluidos = projetos.Count(p => p.StatusProjetoId == (int) StatusProjeto.Concluido),
                TotalProjetos = projetos.Count,
                ProjetosRecentes = projetos
                    .OrderByDescending(p => p.DataCadastro)
                    .Take(5)
                    .Select(p => new ProjetoResumoDto
                    {
                        Id = p.Id,
                        Codigo = p.Codigo,
                        Nome = p.Nome,
                        StatusNome = p.StatusProjetoId.ToString(),
                        DataFimPO = p.DataFimPO,
                        EstaAtrasado = p.EstaAtrasado
                    }).ToList()
            };

            // Calcular porcentagem de entregas no prazo
            var projetosConcluidos = projetos.Where(p => p.StatusProjetoId == (int) StatusProjeto.Concluido).ToList();
            if (projetosConcluidos.Any())
            {
                dashboard.PercentualNoPrazo = Math.Round(
                    (double)projetosConcluidos.Count(p => !p.EstaAtrasado) / projetosConcluidos.Count * 100, 1);
            }

            return Result<DashboardDto>.Success(dashboard);
        }
    }
}
