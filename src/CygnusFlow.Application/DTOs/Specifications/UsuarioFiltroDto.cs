using CygnusFlow.Domain.Specifications;

namespace CygnusFlow.Application.DTOs.Specifications
{
    public class UsuarioFiltroDto
    {
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public int? EquipeId { get; set; }
        public int? TipoUsuarioId { get; set; }
        public int? StatusUsuarioId { get; set; }
        public int Pagina { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 50;

        public UsuarioFiltro ToSpecification()
        {
            return new UsuarioFiltro
            {
                Nome = this.Nome,
                Email = this.Email,
                EquipeId = this.EquipeId,
                TipoUsuarioId = this.TipoUsuarioId,
                StatusUsuarioId = this.StatusUsuarioId,
                Pagina = this.Pagina,
                TamanhoPagina = this.TamanhoPagina
            };
        }
    }
}
