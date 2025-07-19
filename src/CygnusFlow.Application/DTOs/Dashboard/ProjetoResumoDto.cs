using System;

namespace CygnusFlow.Application.DTOs.Dashboard
{
    public class ProjetoResumoDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string StatusNome { get; set; } = string.Empty;
        public DateTime DataFimPO { get; set; }
        public bool EstaAtrasado { get; set; }
    }
}
