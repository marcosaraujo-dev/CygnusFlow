using CygnusFlow.Application.DTOs.Projeto;
using System;
using System.Collections.Generic;

namespace CygnusFlow.Application.DTOs.Reports
{
    public class RelatorioProjetosDto
    {
        public List<ProjetoResponseDto> Projetos { get; set; } = new();
        public int TotalProjetos { get; set; }
        public int ProjetosAtrasados { get; set; }
        public int ProjetosConcluidos { get; set; }
        public double PercentualNoPrazo { get; set; }
        public DateTime DataGeracao { get; set; } = DateTime.Now;
    }
}
