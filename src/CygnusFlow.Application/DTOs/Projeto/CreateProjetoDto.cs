using CygnusFlow.Domain.Enums;
using System;

namespace CygnusFlow.Application.DTOs.Projeto
{
    public class CreateProjetoDto
    {
        public string Nome { get; set; } = string.Empty;
        public int ModuloId { get; set; }
        public Criticidade CriticidadeId { get; set; }
        public DateTime DataInicioPO { get; set; }
        public DateTime DataFimPO { get; set; }
        public int EstimativaHoras { get; set; }
    }
}
