using System;

namespace CygnusFlow.Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public int UsuarioId { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime ValidoAte { get; set; }
    }
}
