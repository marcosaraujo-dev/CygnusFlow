using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CygnusFlow.Infrastructure.Data.Models
{
    [Table("AtividadeComentario")]
    public class AtividadeComentarioModel
    {
        [Key]
        public int Id { get; set; }

        public int AtividadeId { get; set; }
        public int UsuarioId { get; set; }

        [Required]
        public string Comentario { get; set; } = string.Empty;

        public DateTime DataComentario { get; set; } = DateTime.Now;

        [ForeignKey("AtividadeId")]
        public virtual AtividadeModel Atividade { get; set; } = null!;

        [ForeignKey("UsuarioId")]
        public virtual UsuarioModel Usuario { get; set; } = null!;
    }
    
}
