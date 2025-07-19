using System.Collections.Generic;

namespace CygnusFlow.Application.DTOs.Dashboard
{
    public class DashboardDto
    {
        public int ProjetosAtivos { get; set; }
        public int ProjetosAtrasados { get; set; }
        public int ProjetosConcluidos { get; set; }
        public int TotalProjetos { get; set; }
        public double PercentualNoPrazo { get; set; }
        public List<ProjetoResumoDto> ProjetosRecentes { get; set; } = new();
    }
}
