using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CygnusFlow.Domain.Entities
{
    [Table("Criticidade")]
    public class Criticidade
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(50, ErrorMessage = "Nome deve ter no máximo 50 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Descrição deve ter no máximo 200 caracteres")]
        public string? Descricao { get; set; }

        [StringLength(7, ErrorMessage = "Cor deve ter no máximo 7 caracteres (#FFFFFF)")]
        public string? Cor { get; set; }

        public int Nivel { get; set; } // 1 = Baixa, 2 = Média, 3 = Alta, 4 = Crítica
        public bool Ativo { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Projeto> Projetos { get; set; } = new List<Projeto>();
    }
}
