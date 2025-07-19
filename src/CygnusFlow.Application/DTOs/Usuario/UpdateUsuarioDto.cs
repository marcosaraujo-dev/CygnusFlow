using CygnusFlow.Domain.Enums;

namespace CygnusFlow.Application.DTOs.Requests
{
    public class UpdateUsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? EquipeId { get; set; }
        public TipoUsuario TipoUsuarioId { get; set; }
        public StatusUsuario StatusUsuarioId { get; set; }
    }
}
