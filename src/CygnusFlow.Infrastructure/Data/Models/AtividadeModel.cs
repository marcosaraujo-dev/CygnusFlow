using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CygnusFlow.Infrastructure.Data.Models
{
    [Table("Atividade")]
    public class AtividadeModel
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
        public int ProjetoId { get; set; }

        [Required]
        public int ResponsavelId { get; set; }

        [Required]
        public int TipoAtividadeId { get; set; }

        [Column(TypeName = "DATE")]
        public DateTime? DataInicioPlanejada { get; set; }

        [Column(TypeName = "DATE")]
        public DateTime? DataFimPlanejada { get; set; }

        [Column(TypeName = "DATE")]
        public DateTime? DataInicioReal { get; set; }

        [Column(TypeName = "DATE")]
        public DateTime? DataFimReal { get; set; }

        public int StatusProjetoId { get; set; } = 1;

        public string? Observacoes { get; set; }

        public string? Impedimentos { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        // Propriedades de navegação EF
        [ForeignKey("ProjetoId")]
        public virtual ProjetoModel Projeto { get; set; } = null!;

        [ForeignKey("ResponsavelId")]
        public virtual UsuarioModel Responsavel { get; set; } = null!;

        [ForeignKey("TipoAtividadeId")]
        public virtual TipoAtividadeModel TipoAtividade { get; set; } = null!;

        [ForeignKey("StatusProjetoId")]
        public virtual StatusProjetoModel StatusProjeto { get; set; } = null!;

        public virtual ICollection<AtividadeComentarioModel> Comentarios { get; set; } = new List<AtividadeComentarioModel>();
    }
    
}
