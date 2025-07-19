using CygnusFlow.Application.DTOs.Projeto;
using CygnusFlow.Application.DTOs.Reports;
using CygnusFlow.Application.DTOs.Specifications;
using CygnusFlow.Domain.Interfaces.Repositories;
using CygnusFlow.Domain.Shared;
using CygnusFlow.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygnusFlow.Application.Services
{
    public class RelatorioService
    {
        private readonly IProjetoRepository _projetoRepository;

        public RelatorioService(IProjetoRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        public async Task<Result<RelatorioProjetosDto>> GerarRelatorioProjetosAsync(ProjetoFiltroDto filtroDto)
        {
            var filtro = new ProjetoFiltro
            {
                Nome = filtroDto.Nome,
                ModuloId = filtroDto.ModuloId,
                CriticidadeId = filtroDto.CriticidadeId,
                StatusId = filtroDto.StatusId,
                DataInicio = filtroDto.DataInicio,
                DataFim = filtroDto.DataFim
            };

            var result = await _projetoRepository.GetByFiltrosAsync(filtro);

            if (!result.IsSuccess)
                return Result<RelatorioProjetosDto>.Failure(result.Notifications);

            var projetos = result.Items.Select(p => new ProjetoResponseDto
            {
                Id = p.Id,
                Codigo = p.Codigo,
                Nome = p.Nome,
                ModuloNome = p.Modulo?.Nome ?? "",
                CriticidadeNome = p.CriticidadeId.ToString(),
                StatusNome = p.StatusProjetoId.ToString(),
                DataInicioPO = p.DataInicioPO,
                DataFimPO = p.DataFimPO,
                EstimativaHoras = p.EstimativaHoras,
                EstaAtrasado = p.EstaAtrasado(),
                DiasAtraso = p.EstaAtrasado() ? (DateTime.Now.Date - p.DataFimPO.Date).Days : 0,
                DataCadastro = p.DataCadastro
            }).ToList();

            var relatorio = new RelatorioProjetosDto
            {
                Projetos = projetos,
                TotalProjetos = projetos.Count,
                ProjetosAtrasados = projetos.Count(p => p.EstaAtrasado),
                ProjetosConcluidos = projetos.Count(p => p.StatusNome == "Concluido"),
                PercentualNoPrazo = projetos.Any() ?
                    Math.Round((double)projetos.Count(p => !p.EstaAtrasado) / projetos.Count * 100, 1) : 0
            };

            return Result<RelatorioProjetosDto>.Success(relatorio);
        }

        public Result<GanttHtmlDto> GerarGanttHtml(List<ProjetoResponseDto> projetos)
        {
            try
            {
                var html = GerarHtmlGantt(projetos);

                var ganttDto = new GanttHtmlDto
                {
                    ConteudoHtml = html,
                    NomeArquivo = $"gantt_projetos_{DateTime.Now:yyyyMMdd_HHmmss}.html"
                };

                return Result<GanttHtmlDto>.Success(ganttDto);
            }
            catch (Exception ex)
            {
                return Result<GanttHtmlDto>.Failure("Gantt", $"Erro ao gerar Gantt: {ex.Message}");
            }
        }

        private string GerarHtmlGantt(List<ProjetoResponseDto> projetos)
        {
            var sb = new StringBuilder();

            sb.AppendLine(@"
<!DOCTYPE html>
<html lang='pt-BR'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Gráfico Gantt - Projetos</title>
    <script src='https://cdnjs.cloudflare.com/ajax/libs/Chart.js/3.9.1/chart.min.js'></script>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            margin: 0;
            padding: 20px;
            background-color: #f8f9fa;
        }
        .container {
            max-width: 1200px;
            margin: 0 auto;
            background: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }
        .header {
            text-align: center;
            margin-bottom: 30px;
            color: #184194;
        }
        .gantt-container {
            margin: 20px 0;
            border: 1px solid #e5e7eb;
            border-radius: 4px;
            overflow-x: auto;
        }
        .gantt-header {
            background: #f3f4f6;
            padding: 10px;
            font-weight: bold;
            border-bottom: 2px solid #e5e7eb;
        }
        .gantt-row {
            display: flex;
            border-bottom: 1px solid #e5e7eb;
            min-height: 60px;
            align-items: center;
        }
        .gantt-project {
            width: 250px;
            padding: 10px;
            border-right: 1px solid #e5e7eb;
            background: #fafafa;
        }
        .gantt-timeline {
            flex: 1;
            position: relative;
            height: 60px;
            padding: 10px;
        }
        .gantt-bar {
            height: 20px;
            border-radius: 4px;
            position: absolute;
            top: 50%;
            transform: translateY(-50%);
            color: white;
            font-size: 12px;
            line-height: 20px;
            padding: 0 8px;
            overflow: hidden;
            white-space: nowrap;
        }
        .gantt-bar.planejado { background: #6366f1; }
        .gantt-bar.andamento { background: #10b981; }
        .gantt-bar.concluido { background: #059669; }
        .gantt-bar.cancelado { background: #ef4444; }
        .gantt-bar.atrasado { background: #dc2626; }
        .legend {
            display: flex;
            justify-content: center;
            gap: 20px;
            margin: 20px 0;
            font-size: 14px;
        }
        .legend-item {
            display: flex;
            align-items: center;
            gap: 5px;
        }
        .legend-color {
            width: 20px;
            height: 15px;
            border-radius: 2px;
        }
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>📊 Gráfico Gantt - Projetos</h1>
            <p>Gerado em: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + @"</p>
        </div>
        
        <div class='legend'>
            <div class='legend-item'>
                <div class='legend-color planejado'></div>
                <span>Planejado</span>
            </div>
            <div class='legend-item'>
                <div class='legend-color andamento'></div>
                <span>Em Andamento</span>
            </div>
            <div class='legend-item'>
                <div class='legend-color concluido'></div>
                <span>Concluído</span>
            </div>
            <div class='legend-item'>
                <div class='legend-color atrasado'></div>
                <span>Atrasado</span>
            </div>
            <div class='legend-item'>
                <div class='legend-color cancelado'></div>
                <span>Cancelado</span>
            </div>
        </div>
        
        <div class='gantt-container'>
            <div class='gantt-header'>
                <div style='display: flex;'>
                    <div style='width: 250px;'>Projeto</div>
                    <div style='flex: 1; text-align: center;'>Timeline (Data de Início → Data de Fim)</div>
                </div>
            </div>");

            // Calcular período total
            var dataMinima = projetos.Min(p => p.DataInicioPO);
            var dataMaxima = projetos.Max(p => p.DataFimPO);
            var totalDias = (dataMaxima - dataMinima).Days;

            foreach (var projeto in projetos.OrderBy(p => p.DataInicioPO))
            {
                var diasInicio = (projeto.DataInicioPO - dataMinima).Days;
                var duracaoProjeto = (projeto.DataFimPO - projeto.DataInicioPO).Days;

                var porcentagemInicio = totalDias > 0 ? (double)diasInicio / totalDias * 100 : 0;
                var porcentagemLargura = totalDias > 0 ? (double)duracaoProjeto / totalDias * 100 : 100;

                var cssClass = projeto.StatusNome.ToLower() switch
                {
                    "planejado" => "planejado",
                    "emandamento" => "andamento",
                    "concluido" => "concluido",
                    "cancelado" => "cancelado",
                    _ when projeto.EstaAtrasado => "atrasado",
                    _ => "planejado"
                };

                sb.AppendLine($@"
            <div class='gantt-row'>
                <div class='gantt-project'>
                    <div style='font-weight: bold; font-size: 14px;'>{projeto.Codigo}</div>
                    <div style='font-size: 12px; color: #666;'>{projeto.Nome}</div>
                    <div style='font-size: 11px; color: #999;'>{projeto.DataInicioPO:dd/MM/yyyy} - {projeto.DataFimPO:dd/MM/yyyy}</div>
                </div>
                <div class='gantt-timeline'>
                    <div class='gantt-bar {cssClass}' 
                         style='left: {porcentagemInicio:F1}%; width: {Math.Max(porcentagemLargura, 2):F1}%;'>
                        {projeto.Codigo}
                    </div>
                </div>
            </div>");
            }

            sb.AppendLine(@"
        </div>
        
        <div style='margin-top: 30px; padding: 20px; background: #f9fafb; border-radius: 4px;'>
            <h3>📈 Estatísticas</h3>
            <div style='display: grid; grid-template-columns: repeat(4, 1fr); gap: 20px; text-align: center;'>
                <div>
                    <div style='font-size: 24px; font-weight: bold; color: #184194;'>" + projetos.Count + @"</div>
                    <div style='color: #666;'>Total de Projetos</div>
                </div>
                <div>
                    <div style='font-size: 24px; font-weight: bold; color: #10b981;'>" + projetos.Count(p => !p.EstaAtrasado) + @"</div>
                    <div style='color: #666;'>No Prazo</div>
                </div>
                <div>
                    <div style='font-size: 24px; font-weight: bold; color: #ef4444;'>" + projetos.Count(p => p.EstaAtrasado) + @"</div>
                    <div style='color: #666;'>Atrasados</div>
                </div>
                <div>
                    <div style='font-size: 24px; font-weight: bold; color: #059669;'>" + projetos.Count(p => p.StatusNome == "Concluido") + @"</div>
                    <div style='color: #666;'>Concluídos</div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>");

            return sb.ToString();
        }
    }
}
