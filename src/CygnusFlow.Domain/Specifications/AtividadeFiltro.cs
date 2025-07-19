using System;

namespace CygnusFlow.Domain.Specifications
{
    public class AtividadeFiltro
    {
        public string? Nome { get; set; }
        public int? ProjetoId { get; set; }
        public int? ResponsavelId { get; set; }
        public int? TipoAtividadeId { get; set; }
        public int? StatusId { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public bool? ApenasAtrasadas { get; set; }
        public int Pagina { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 50;
    }
}
