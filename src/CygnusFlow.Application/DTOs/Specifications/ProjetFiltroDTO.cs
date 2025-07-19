using CygnusFlow.Domain.Specifications;
using System;

namespace CygnusFlow.Application.DTOs.Specifications
{
    public class ProjetoFiltroDto
    {
        public string? Nome { get; set; }
        public int? ModuloId { get; set; }
        public int? CriticidadeId { get; set; }
        public int? StatusId { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int Pagina { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 50;

        // Método para converter para Specification
        public ProjetoFiltro ToSpecification()
        {
            return new ProjetoFiltro
            {
                Nome = this.Nome,
                ModuloId = this.ModuloId,
                CriticidadeId = this.CriticidadeId,
                StatusId = this.StatusId,
                DataInicio = this.DataInicio,
                DataFim = this.DataFim,
                Pagina = this.Pagina,
                TamanhoPagina = this.TamanhoPagina
            };
        }
    }
}
