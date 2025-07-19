using System;

namespace CygnusFlow.Application.DTOs.Projeto
{
    public class ProjetoListDto
    {
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string ModuloNome { get; set; } = string.Empty;
        public string CriticidadeNome { get; set; } = string.Empty;
        public string StatusNome { get; set; } = string.Empty;
        public DateTime DataFimPO { get; set; }
        public bool EstaAtrasado { get; set; }
    }
}
