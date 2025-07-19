using CygnusFlow.Application.DTOs.Atividade;
using System;
using System.Collections.Generic;

namespace CygnusFlow.Application.DTOs.Reports
{
    public class RelatorioAtividadesDto
    {
        public List<AtividadeResponseDto> Atividades { get; set; } = new();
        public int TotalAtividades { get; set; }
        public int AtividadesAtrasadas { get; set; }
        public int AtividadesConcluidas { get; set; }
        public double PercentualNoPrazo { get; set; }
        public DateTime DataGeracao { get; set; } = DateTime.Now;
    }
}
