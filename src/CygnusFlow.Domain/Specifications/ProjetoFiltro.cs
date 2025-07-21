using System;

namespace CygnusFlow.Domain.Specifications
{
    public class ProjetoFiltro
    {
        public string? Nome { get; set; }
        public int? ModuloId { get; set; }
        public int? CriticidadeId { get; set; }
        public int? StatusId { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        public string Codigo { get; set; } = string.Empty;
        public bool? ApenasAtrasados { get; set; }
        public int Pagina { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 50;
    }
}
