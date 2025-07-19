using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CygnusFlow.Domain.Specifications
{
    public class UsuarioFiltro
    {
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public int? EquipeId { get; set; }
        public int? TipoUsuarioId { get; set; }
        public int? StatusUsuarioId { get; set; }
        public int Pagina { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 50;
    }
}
