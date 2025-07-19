using CygnusFlow.Domain.Enums;
using System;

namespace CygnusFlow.Application.DTOs.Atividade
{
    public class CreateAtividadeDto
    {
        public string Nome { get; set; } = string.Empty;
        public int ProjetoId { get; set; }
        public int ResponsavelId { get; set; }
        public TipoAtividade TipoAtividadeId { get; set; }
        public DateTime? DataInicioPlanejada { get; set; }
        public DateTime? DataFimPlanejada { get; set; }
        public string? Observacoes { get; set; }
    }
}
