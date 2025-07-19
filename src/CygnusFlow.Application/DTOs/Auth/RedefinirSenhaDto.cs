namespace CygnusFlow.Application.DTOs.Auth
{
    public class RedefinirSenhaDto
    {
        public string Email { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string NovaSenha { get; set; } = string.Empty;
        public string ConfirmarSenha { get; set; } = string.Empty;
    }
}
