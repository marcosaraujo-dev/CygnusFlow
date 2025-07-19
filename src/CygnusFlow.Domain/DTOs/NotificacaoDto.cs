using System;

namespace CygnusFlow.Domain.DTOs
{
    public class NotificacaoDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public bool Lida { get; set; }
        public int UsuarioId { get; set; }
        public string? IconeUrl { get; set; }
        public string? AcaoUrl { get; set; }
    }
}
