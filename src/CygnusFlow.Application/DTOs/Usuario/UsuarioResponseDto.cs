using System;

namespace CygnusFlow.Application.DTOs.Usuario
{
    public class UsuarioResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string EquipeNome { get; set; } = string.Empty;
        public string TipoUsuarioNome { get; set; } = string.Empty;
        public string StatusUsuarioNome { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; }
    }
}
