using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Specifications
{
    public class AuditFiltro
    {
        public int? UsuarioId { get; set; }
        public string? Acao { get; set; }
        public string? Entidade { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int Pagina { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 50;
    }
}
