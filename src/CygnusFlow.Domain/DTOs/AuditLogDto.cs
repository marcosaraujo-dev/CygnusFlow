using System;

namespace CygnusFlow.Domain.DTOs
{
    public class AuditLogDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public string Acao { get; set; } = string.Empty;
        public string Entidade { get; set; } = string.Empty;
        public int EntidadeId { get; set; }
        public string Detalhes { get; set; } = string.Empty;
        public DateTime DataHora { get; set; }
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
    }
}
