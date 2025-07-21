using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CygnusFlow.Infrastructure.Data.Models
{
    [Table("Projeto")]
    public class ProjetoModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public int ModuloId { get; set; }

        [Required]
        public int CriticidadeId { get; set; }

        [Required]
        [Column(TypeName = "DATE")]
        public DateTime DataInicioPO { get; set; }

        [Required]
        [Column(TypeName = "DATE")]
        public DateTime DataFimPO { get; set; }

        [Required]
        public int EstimativaHoras { get; set; }

        public int StatusProjetoId { get; set; } = 1;

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        // Propriedades de navegação EF
        [ForeignKey("ModuloId")]
        public virtual ModuloSistemaModel Modulo { get; set; } = null!;

        [ForeignKey("CriticidadeId")]
        public virtual CriticidadeModel Criticidade { get; set; } = null!;

        [ForeignKey("StatusProjetoId")]
        public virtual StatusProjetoModel StatusProjeto { get; set; } = null!;

        // Coleções relacionadas
        public virtual ICollection<AtividadeModel> Atividades { get; set; } = new List<AtividadeModel>();
        public virtual ICollection<ProjetoComentarioModel> Comentarios { get; set; } = new List<ProjetoComentarioModel>();
    }
    
}
