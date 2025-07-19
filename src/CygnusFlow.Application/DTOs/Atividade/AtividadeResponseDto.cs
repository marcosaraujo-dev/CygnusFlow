using System;

namespace CygnusFlow.Application.DTOs.Atividade
{

    public class AtividadeResponseDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string ProjetoNome { get; set; } = string.Empty;
        public string ResponsavelNome { get; set; } = string.Empty;
        public string StatusNome { get; set; } = string.Empty;
        public DateTime? DataFimPlanejada { get; set; }
        public DateTime? DataFimReal { get; set; }
        public bool EstaAtrasada { get; set; }
        public int DiasAtraso { get; set; }
    }
}
